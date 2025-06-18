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

        //무기 교체 상태
        private WeaponSwitchState weaponSwitchState;

        //새로 교체할 무기의 인덱스
        private int weaponSwitchNewWeaponIndex;

        private float weaponSwitchTimeStarted = 0f;     //연출 시작 시간
        private float weaponSwitchDelay = 1f;           //연출 플레이 시간

        //적 포착
        public Camera weaponCamera;
        #endregion

        #region Property
        //무기 리스트(weaponSlots)를 관리하는 인덱스 - 현재 액티브한 무기의 인덱스
        public int ActiveWeaponIndex { get; private set; }

        //적 포착 체크
        public bool IsPointingAtEnemy { get; private set; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            inputHandler = this.GetComponent<PlayerInputHandler>();

            //초기화
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;

            //무기 교체시 호출될 함수 등록
            OnSwitchToWeapon += OnWeaponSwitched;

            //처음 지급 받은 무기를 장착한다
            foreach(var w in startingWeapons)
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
            //무기 교체 연출
            UpdateWeaponState();

            //무기의 최종 위치
            weaponParentSocket.localPosition = weaponMainLocalPosition;
        }
        #endregion

        #region Custom Method
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
        #endregion
    }

}
