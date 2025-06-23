using System;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    //무기 교체 상태
    public enum WeaponSwitchState
    {
        Up,     //무기가 액티브해서 들려져 있는 상태
        Down,   //무기가 언액티브해서 내려져 있는 상태
        PutDownPrevious,    //무기가 내려지기 이전 상태
        PutUpNew,   //새로 올리는 상태
    }

    //플레이어가 가진 무기들을 관리하는 클래스
    public class PlayerWeaponManager : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerInputHandler inputHandler;
        private PlayerCharacterController playerCharacterController;

        //시작 할 때 지급 되는 무기 : 3개 지급
        public List<WeaponController> startingWeapons = new List<WeaponController>();

        //무기 장착
        //무기가 장착되는 오브젝트
        public Transform weaponParentSocket;

        //플레이어가 게임중에 들고 다니는 무기 리스트
        private WeaponController[] weaponSlots = new WeaponController[9];

        //무기 위치
        private Vector3 weaponMainLocalPosition;

        //무기 교체
        //무기 교체시 등록된 함수들이 호출되는 Unity Action함수
        public UnityAction<WeaponController> OnSwitchToWeapon;

        //무기 교체
        //무기 교체시 계산되는 위치
        public Transform defaultWeaponPosition;
        public Transform downWeaponPosition;
        public Transform aimingWeaponPosition;

        //무기 교체 상태
        private WeaponSwitchState weaponSwitchState;

        //새로 교체할 무기의 인덱스
        private int weaponSwitchNewWeaponIndex;

        private float weaponSwitchTimeStarted = 0f;     //연출 시작 시간
        private float weaponSwitchDelay = 1f;           //연출 플레이 시간

        //적 포착
        public Camera weaponCamera;

        //카메라 FOV
        [SerializeField]
        private float defaultFov = 60f;
        //무기에 따른 fov 즘 계수 값
        [SerializeField]
        private float weaponFovMultiplier = 1f;

        //조준 Aim
        private float aimingAnimationSpeed = 10f;   //조준 이동 연출
        private float aimingFov;

        //흔들림 bob
        [SerializeField]
        private float bobFrequency = 10f;
        [SerializeField]
        private float bobSharpness = 10f;
        [SerializeField]
        private float defaultBobAmount = 0.05f;     //기본 흔들림 값
        [SerializeField]
        private float aimingBobAmount = 0.02f;      //조준시 흔들림 값

        private float m_WeaponBobFactor;            //이동 속도(매 프레임)에 따른 흔들림 계수
        private Vector3 m_LastCharacterPosition;    //이번 프레임의 캐릭터 최종위치

        private Vector3 m_WeaponBobLocalPosition;   //이번 프레임에 흔들림 량의 최종 계산값

        //반동 Recoil
        [SerializeField]
        private float recoilSharpness = 50f;        //반동 연출 속도
        [SerializeField]
        private float maxRecoilDistance = 0.5f;     //반동시 뒤로 밀리는 최대거리
        [SerializeField]
        private float recoilRepositionSharpness = 10f;  //반동 연출 회복 속도
        private Vector3 accumulateRecoil;           //반동 힘에 의한 이동 Vector3 값

        private Vector3 weaponRecoilLocalPosition;  //반동에 의해 이동한 최종 게산값

        //저격모드 Sniper
        private bool isScopeOn = false;
        //저격모드 시작/해제 시 등록된 함수를 호출하는 이벤트 함수
        public UnityAction OnScopeWeapon;
        public UnityAction OffScopeWeapon;
        #endregion

        #region Property
        //무기 리스트(weaponSlots)를 관리하는 인덱스 - 현재 액티브한 무기의 인덱스
        public int ActiveWeaponIndex { get; private set; }

        //적 포착 체크
        public bool IsPointingAtEnemy { get; private set; }

        //조준 여부 체크
        public bool IsAiming { get; private set; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            inputHandler = this.GetComponent<PlayerInputHandler>();
            playerCharacterController = this.GetComponent<PlayerCharacterController>();

            //초기화
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;

            SetFov(defaultFov);

            //무기 교체시 호출될 함수 등록
            OnSwitchToWeapon += OnWeaponSwitched;

            //저격모드 On/Off 시 호출될 함수 등록
            OnScopeWeapon += OnScope;
            OffScopeWeapon += OffScope;

            //처음 지급 받은 무기를 장착한다
            foreach (var w in startingWeapons)
            {
                //무기 리스트 추가
                AddWeapon(w);
            }
            //무기 교체
            SwitchWeapon(true);
        }
        private void Update()
        {
            //현재 액티브 무기 가져오기
            WeaponController activeWeapon = GetActiveWeapon();

            if(weaponSwitchState == WeaponSwitchState.Up)
            {
                //키 인풋 받아서 조준
                IsAiming = inputHandler.GetAimInputHeld();

                if(activeWeapon.ShootType == WeaponShootType.Sniper)
                {
                    if (inputHandler.GetAimInputDown())
                    {
                        //저격 모드 시작
                        isScopeOn = true;
                        OnScope();
                    }
                    else if (inputHandler.GetAimInputReleased())
                    {
                        //저격 모드 해제
                        OffScopeWeapon?.Invoke();
                        OffScope();
                    }
                }

                //발사
                bool isFire = activeWeapon.HandleShootInput(
                    inputHandler.GetFireInputDown(), 
                    inputHandler.GetFireInputHeld(), 
                    inputHandler.GetFireInputRealeased());

                //발사 성공시 총을 뒤로 밀린다
                if (isFire)
                {
                    accumulateRecoil += Vector3.back * activeWeapon.recoilForce;
                    accumulateRecoil = Vector3.ClampMagnitude(accumulateRecoil, maxRecoilDistance);
                }
            }

            if(weaponSwitchState == WeaponSwitchState.Up || weaponSwitchState == WeaponSwitchState.Down)
            {
                //키 인풋을 받아 무기 교체
                int switchWeaponInput = inputHandler.GetSwitchWeaponInput();
                if (switchWeaponInput != 0)
                {
                    bool switchUp = switchWeaponInput > 0f;
                    //무기 교체
                    SwitchWeapon(switchUp);
                }
            }
            //적 포착
            IsPointingAtEnemy = false;
            if (activeWeapon)
            {
                if (Physics.Raycast(weaponCamera.transform.position, weaponCamera.transform.forward, out RaycastHit hit, 1000))
                {
                    //충돌체중에서 적을 판정
                    if(hit.collider.GetComponentInParent<Health>() != null)
                    {
                        IsPointingAtEnemy = true;
                    }                    
                }
            }
            
        }
        private void LateUpdate()
        {
            //반동 효과 연출
            UpdateWeaponRecoil();

            //무기 교체 연출
            UpdateWeaponState();

            //무기 흔들림
            UpdateWeaponBob();

            //무기 조준
            UpdateWeaponAiming();

            //무기의 최종 위치
            weaponParentSocket.localPosition = weaponMainLocalPosition + m_WeaponBobLocalPosition + weaponRecoilLocalPosition;
        }
        #endregion

        #region Custom Method
        //카메라 FOV 조정
        private void SetFov(float fov)
        {
            playerCharacterController.PlayerCamera.fieldOfView = fov;
            weaponCamera.fieldOfView = fov * weaponFovMultiplier;
        }

        //조준 연출에 따른 무기 위치 변경
        private void UpdateWeaponAiming()
        {
            if (weaponSwitchState != WeaponSwitchState.Up)
                return;

            WeaponController activeWeapon = GetActiveWeapon();

            if (IsAiming && activeWeapon)   //조준
            {
                if (isScopeOn)
                {
                    //거리 측정 후 isScopeOn = false
                    float distance = Vector3.Distance(weaponMainLocalPosition, aimingWeaponPosition.localPosition + activeWeapon.aimOffset);
                    if (distance < 0.05f)
                    {
                        //저격 모드 시작
                        isScopeOn = false;

                        //ScopeUI 활성화, 무기가 안보이도록 만들기
                        OnScopeWeapon?.Invoke();
                    }
                }

                //위치 조정
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition, aimingWeaponPosition.localPosition + activeWeapon.aimOffset, aimingAnimationSpeed * Time.deltaTime);
                //Fov 조정
                if(isScopeOn == false)
                {
                    aimingFov = Mathf.Lerp(playerCharacterController.PlayerCamera.fieldOfView, defaultFov * activeWeapon.aimZoomRatio, aimingAnimationSpeed * Time.deltaTime);
                    SetFov(aimingFov);
                }
               
            }
            else  //조준X
            {
                //위치 조정
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition, defaultWeaponPosition.localPosition, aimingAnimationSpeed * Time.deltaTime);
                //Fov 조정
                aimingFov = Mathf.Lerp(playerCharacterController.PlayerCamera.fieldOfView, defaultFov, aimingAnimationSpeed * Time.deltaTime);
                SetFov(aimingFov);
            }
        }

        //반동 연출에 따른 무기가 뒤로 밀린량 구하기
        private void UpdateWeaponRecoil()
        {
            //accumulateRecoil: 힘에 의해 뒤로 밀린 량
            //weaponRecoilLocalPosition: 뒤로 밀린 량
            if(weaponRecoilLocalPosition.z >= accumulateRecoil.z * 0.99f)   //뒤로 밀리는 연출
            {
                weaponRecoilLocalPosition = Vector3.Lerp(weaponRecoilLocalPosition, accumulateRecoil, recoilSharpness * Time.deltaTime);
            }
            else
            {
                weaponRecoilLocalPosition = Vector3.Lerp(weaponRecoilLocalPosition, Vector3.zero, recoilRepositionSharpness * Time.deltaTime);
                accumulateRecoil = weaponRecoilLocalPosition;
            }
        }

        //흔들림에 따른 무기 흔들림 량 구하기
        private void UpdateWeaponBob()
        {
            if(Time.deltaTime > 0)
            {
                //이번 프레임에 이동한 거리
                Vector3 playerCharacterVelocity = (playerCharacterController.transform.position - m_LastCharacterPosition) / Time.deltaTime;

                //게임에서 캐릭터의 이동 속도 계수(0~1) 정지 : 0 , Max 이동속도 시 1
                float charactorMovementFactor = 0f;
                if (playerCharacterController.IsGrounded)
                {
                    charactorMovementFactor = Mathf.Clamp01(playerCharacterVelocity.magnitude / 
                        (playerCharacterController.MaxSpeedOnGround * playerCharacterController.SprintSpeedModifier));
                }

                m_WeaponBobFactor = Mathf.Lerp(m_WeaponBobFactor, charactorMovementFactor, bobSharpness * Time.deltaTime);

                //BobFactor에 따른 흔들림 량, 흔들림 속도로 최종적인 m_WeaponBobLocalPosition  구하기
                float bobAmount = IsAiming ? aimingBobAmount : defaultBobAmount;
                float frequency = bobFrequency;
                //좌 우 이동량
                float hBobValue = Mathf.Sin(Time.time * frequency) * bobAmount * m_WeaponBobFactor;
                //위 아래 이동량
                float vBobValue = (Mathf.Sin(Time.time * frequency * 2) * 0.5f + 0.5f) * bobAmount * m_WeaponBobFactor;

                //흔들림 최종 위치
                m_WeaponBobLocalPosition.x = hBobValue;
                m_WeaponBobLocalPosition.y = vBobValue;
                //
                m_LastCharacterPosition = playerCharacterController.transform.position;
            }
        }

        //무기 교체 연출 및 상태 변경 구현
        private void UpdateWeaponState()
        {
            //Lerp t변수
            float switchingTimeFactor = 0f;
            if (weaponSwitchDelay <= 0f)
            {
                switchingTimeFactor = 1f;
            }
            else
            {
                switchingTimeFactor = Mathf.Clamp01((Time.time - weaponSwitchTimeStarted) / weaponSwitchDelay);
            }

            //타이머가 완료되었을때 연출 완료하고 상태 변경
            if(switchingTimeFactor >= 1f)
            {
                //디폴트 위치에서 아래 위치로 이동 완료한 상태
                if (weaponSwitchState == WeaponSwitchState.PutDownPrevious)
                {
                    //무기 교체 : 이전 무기 false,새로운 무기 true
                    WeaponController oldWeapon = GetWeaponSlotIndex(ActiveWeaponIndex);
                    if(oldWeapon != null)
                    {
                        oldWeapon.ShowWeapon(false);
                    }
                    //새로운 무기 인덱스를 액티브 인덱스로 저장
                    ActiveWeaponIndex = weaponSwitchNewWeaponIndex;

                    //액티브 인덱스 해당되는 무기(weaponController)가져오기
                    WeaponController newWeapon = GetWeaponSlotIndex(ActiveWeaponIndex);
                    //액티브 무기(weaponController)를 매개변수로 한 등록된 함수들 호출
                    OnSwitchToWeapon?.Invoke(newWeapon);

                    switchingTimeFactor = 0f;
                    if(newWeapon != null)   //새로운 무기가 있으면 연출 시작
                    {
                        weaponSwitchTimeStarted = Time.time;
                        weaponSwitchState = WeaponSwitchState.PutUpNew;
                    }
                    else    //새로운 무기가 없을 때
                    {
                        weaponSwitchState = WeaponSwitchState.Down;
                    }
                }
                else if (weaponSwitchState == WeaponSwitchState.PutUpNew)
                {
                    //
                    weaponSwitchState = WeaponSwitchState.Up;
                }
            }
            else    //0 -> 1 연출 중
            {
                if(weaponSwitchState == WeaponSwitchState.PutDownPrevious)
                {
                    weaponMainLocalPosition = Vector3.Lerp(defaultWeaponPosition.localPosition, downWeaponPosition.localPosition, switchingTimeFactor);
                }
                else if (weaponSwitchState == WeaponSwitchState.PutUpNew)
                {
                    weaponMainLocalPosition = Vector3.Lerp(downWeaponPosition.localPosition, defaultWeaponPosition.localPosition, switchingTimeFactor);
                }
            }
        }

        //매개변수로 받은 무기(Weapon Prefab)를 무기 리스트에 추가
        private bool AddWeapon(WeaponController weaponPrefab)
        {
            //새로 추가하는 무기 소지 여부 - 중복 검사
            if(HasWeapon(weaponPrefab) != null)
            {
                Debug.Log("Has Same Weapon");
                return false;
            }

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                //빈 슬롯 찾기
                if (weaponSlots[i] == null)
                {
                    WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                    weaponInstance.transform.localPosition = Vector3.zero;
                    weaponInstance.transform.localRotation = Quaternion.identity;

                    weaponInstance.Owner = this.gameObject;
                    weaponInstance.SourcePrefab = weaponPrefab.gameObject;
                    weaponInstance.ShowWeapon(false);

                    weaponSlots[i] = weaponInstance;
                    return true;
                }
                
            }
            Debug.Log("WeaponSlots Full");
            return false;
        }

        //매개변수로 받은 프리팹으로 생성된 무기가 있으면 생성된 무기를 반환
        private WeaponController HasWeapon(WeaponController weaponPrefab)
        {
            foreach (var w in weaponSlots)
            {
                if(w != null && w.SourcePrefab == weaponPrefab)
                {
                    return w;
                }
            }

            return null;
        }

        //현재 액티브한 무기 가져오기
        public WeaponController GetActiveWeapon()
        {
            return GetWeaponSlotIndex(ActiveWeaponIndex);
        }

        //지정 인덱스의 슬롯 가져오기
        private WeaponController GetWeaponSlotIndex(int index)
        {
            if (index < 0 || index >= weaponSlots.Length)
                return null;

            return weaponSlots[index];
        }

        //현재 들고 있는 무기 false, 새로운 무기 true;
        //ascendingOrder 다음 무기 가져오는 기준 : 인덱스의 오름차순, 내림차순
        private void SwitchWeapon(bool ascendingOrder)
        {
            //새로운 무기의 인덱스
            int newWeaponIndex = -1;
            //현재 활성화한 무기의 가장 가까운 무기 찾기
            int closestSlotDistance = weaponSlots.Length;

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (i != ActiveWeaponIndex && GetWeaponSlotIndex(i) != null)
                {
                    int distanceToActiveIndex = GetDistanceBetweenWeaponSlots(ActiveWeaponIndex, i, ascendingOrder);
                    if (distanceToActiveIndex < closestSlotDistance)
                    {
                        closestSlotDistance = distanceToActiveIndex;
                        newWeaponIndex = i;
                    }
                }  
            }

            //새로운 무기의 인덱스로 무기 교체
            SwitchToWeaponIndex(newWeaponIndex);
        }

        //매개변수로 받은 무기로 교체
        private void SwitchToWeaponIndex(int newWeaponIndex)
        {
            if (newWeaponIndex == ActiveWeaponIndex)
                return;

            if (newWeaponIndex < 0 || newWeaponIndex >= weaponSlots.Length)
                return;

            weaponSwitchNewWeaponIndex = newWeaponIndex;
            //연출시작 시간 저장
            weaponSwitchTimeStarted = Time.time;

            if(GetActiveWeapon() == null)
            {
                //무기 위치를 아래 위치에 가져다 놓는다
                weaponMainLocalPosition = downWeaponPosition.localPosition;
                //올리는 상태로 변경
                weaponSwitchState = WeaponSwitchState.PutUpNew;
                //새로운 무기 인덱스를 액티브 인덱스로 저장
                ActiveWeaponIndex = newWeaponIndex;

                //액티브 인덱스 해당되는 무기(weaponController)가져오기
                WeaponController weaponController = GetWeaponSlotIndex(ActiveWeaponIndex);
                //액티브 무기(weaponController)를 매개변수로 한 등록된 함수들 호출
                OnSwitchToWeapon?.Invoke(weaponController);
            }
            else
            {
                weaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }

        //무기 슬롯간의 거리 구하기
        private int GetDistanceBetweenWeaponSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
        {
            int distance = 0;

            if(ascendingOrder == true)
            {
                distance = toSlotIndex - fromSlotIndex;
            }
            else
            {
                distance = -1 * (toSlotIndex - fromSlotIndex);
            }
            if(distance < 0)
            {
                distance = distance + weaponSlots.Length;
            }
                return distance;
        }

        //매개변수로 받은 무기로 교체
        private void OnWeaponSwitched(WeaponController newWeapon)
        {
            if(newWeapon != null)
            {
                newWeapon.ShowWeapon(true);
            }
        }

        //무기 전용 카메라 On
        private void OnScope()
        {
            weaponCamera.enabled = false;
        }
        private void OffScope()
        {
            weaponCamera.enabled = true;
        }
        #endregion
    }

}
