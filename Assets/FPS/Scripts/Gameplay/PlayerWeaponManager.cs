using System;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    //���� ��ü ����
    public enum WeaponSwitchState
    {
        Up,     //���Ⱑ ��Ƽ���ؼ� ����� �ִ� ����
        Down,   //���Ⱑ ���Ƽ���ؼ� ������ �ִ� ����
        PutDownPrevious,    //���Ⱑ �������� ���� ����
        PutUpNew,   //���� �ø��� ����
    }

    //�÷��̾ ���� ������� �����ϴ� Ŭ����
    public class PlayerWeaponManager : MonoBehaviour
    {
        #region Variables
        //����
        private PlayerInputHandler inputHandler;
        private PlayerCharacterController playerCharacterController;

        //���� �� �� ���� �Ǵ� ���� : 3�� ����
        public List<WeaponController> startingWeapons = new List<WeaponController>();

        //���� ����
        //���Ⱑ �����Ǵ� ������Ʈ
        public Transform weaponParentSocket;

        //�÷��̾ �����߿� ��� �ٴϴ� ���� ����Ʈ
        private WeaponController[] weaponSlots = new WeaponController[9];

        //���� ��ġ
        private Vector3 weaponMainLocalPosition;

        //���� ��ü
        //���� ��ü�� ��ϵ� �Լ����� ȣ��Ǵ� Unity Action�Լ�
        public UnityAction<WeaponController> OnSwitchToWeapon;

        //���� ��ü
        //���� ��ü�� ���Ǵ� ��ġ
        public Transform defaultWeaponPosition;
        public Transform downWeaponPosition;
        public Transform aimingWeaponPosition;

        //���� ��ü ����
        private WeaponSwitchState weaponSwitchState;

        //���� ��ü�� ������ �ε���
        private int weaponSwitchNewWeaponIndex;

        private float weaponSwitchTimeStarted = 0f;     //���� ���� �ð�
        private float weaponSwitchDelay = 1f;           //���� �÷��� �ð�

        //�� ����
        public Camera weaponCamera;

        //ī�޶� FOV
        [SerializeField]
        private float defaultFov = 60f;
        //���⿡ ���� fov �� ��� ��
        [SerializeField]
        private float weaponFovMultiplier = 1f;

        //���� Aim
        private float aimingAnimationSpeed = 10f;   //���� �̵� ����
        private float aimingFov;

        //��鸲 bob
        [SerializeField]
        private float bobFrequency = 10f;
        [SerializeField]
        private float bobSharpness = 10f;
        [SerializeField]
        private float defaultBobAmount = 0.05f;     //�⺻ ��鸲 ��
        [SerializeField]
        private float aimingBobAmount = 0.02f;      //���ؽ� ��鸲 ��

        private float m_WeaponBobFactor;            //�̵� �ӵ�(�� ������)�� ���� ��鸲 ���
        private Vector3 m_LastCharacterPosition;    //�̹� �������� ĳ���� ������ġ

        private Vector3 m_WeaponBobLocalPosition;   //�̹� �����ӿ� ��鸲 ���� ���� ��갪

        //�ݵ� Recoil
        [SerializeField]
        private float recoilSharpness = 50f;        //�ݵ� ���� �ӵ�
        [SerializeField]
        private float maxRecoilDistance = 0.5f;     //�ݵ��� �ڷ� �и��� �ִ�Ÿ�
        [SerializeField]
        private float recoilRepositionSharpness = 10f;  //�ݵ� ���� ȸ�� �ӵ�
        private Vector3 accumulateRecoil;           //�ݵ� ���� ���� �̵� Vector3 ��

        private Vector3 weaponRecoilLocalPosition;  //�ݵ��� ���� �̵��� ���� �Ի갪

        //���ݸ�� Sniper
        private bool isScopeOn = false;
        //���ݸ�� ����/���� �� ��ϵ� �Լ��� ȣ���ϴ� �̺�Ʈ �Լ�
        public UnityAction OnScopeWeapon;
        public UnityAction OffScopeWeapon;
        #endregion

        #region Property
        //���� ����Ʈ(weaponSlots)�� �����ϴ� �ε��� - ���� ��Ƽ���� ������ �ε���
        public int ActiveWeaponIndex { get; private set; }

        //�� ���� üũ
        public bool IsPointingAtEnemy { get; private set; }

        //���� ���� üũ
        public bool IsAiming { get; private set; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            inputHandler = this.GetComponent<PlayerInputHandler>();
            playerCharacterController = this.GetComponent<PlayerCharacterController>();

            //�ʱ�ȭ
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;

            SetFov(defaultFov);

            //���� ��ü�� ȣ��� �Լ� ���
            OnSwitchToWeapon += OnWeaponSwitched;

            //���ݸ�� On/Off �� ȣ��� �Լ� ���
            OnScopeWeapon += OnScope;
            OffScopeWeapon += OffScope;

            //ó�� ���� ���� ���⸦ �����Ѵ�
            foreach (var w in startingWeapons)
            {
                //���� ����Ʈ �߰�
                AddWeapon(w);
            }
            //���� ��ü
            SwitchWeapon(true);
        }
        private void Update()
        {
            //���� ��Ƽ�� ���� ��������
            WeaponController activeWeapon = GetActiveWeapon();

            if(weaponSwitchState == WeaponSwitchState.Up)
            {
                //Ű ��ǲ �޾Ƽ� ����
                IsAiming = inputHandler.GetAimInputHeld();

                if(activeWeapon.ShootType == WeaponShootType.Sniper)
                {
                    if (inputHandler.GetAimInputDown())
                    {
                        //���� ��� ����
                        isScopeOn = true;
                        OnScope();
                    }
                    else if (inputHandler.GetAimInputReleased())
                    {
                        //���� ��� ����
                        OffScopeWeapon?.Invoke();
                        OffScope();
                    }
                }

                //�߻�
                bool isFire = activeWeapon.HandleShootInput(
                    inputHandler.GetFireInputDown(), 
                    inputHandler.GetFireInputHeld(), 
                    inputHandler.GetFireInputRealeased());

                //�߻� ������ ���� �ڷ� �и���
                if (isFire)
                {
                    accumulateRecoil += Vector3.back * activeWeapon.recoilForce;
                    accumulateRecoil = Vector3.ClampMagnitude(accumulateRecoil, maxRecoilDistance);
                }
            }

            if(weaponSwitchState == WeaponSwitchState.Up || weaponSwitchState == WeaponSwitchState.Down)
            {
                //Ű ��ǲ�� �޾� ���� ��ü
                int switchWeaponInput = inputHandler.GetSwitchWeaponInput();
                if (switchWeaponInput != 0)
                {
                    bool switchUp = switchWeaponInput > 0f;
                    //���� ��ü
                    SwitchWeapon(switchUp);
                }
            }
            //�� ����
            IsPointingAtEnemy = false;
            if (activeWeapon)
            {
                if (Physics.Raycast(weaponCamera.transform.position, weaponCamera.transform.forward, out RaycastHit hit, 1000))
                {
                    //�浹ü�߿��� ���� ����
                    if(hit.collider.GetComponentInParent<Health>() != null)
                    {
                        IsPointingAtEnemy = true;
                    }                    
                }
            }
            
        }
        private void LateUpdate()
        {
            //�ݵ� ȿ�� ����
            UpdateWeaponRecoil();

            //���� ��ü ����
            UpdateWeaponState();

            //���� ��鸲
            UpdateWeaponBob();

            //���� ����
            UpdateWeaponAiming();

            //������ ���� ��ġ
            weaponParentSocket.localPosition = weaponMainLocalPosition + m_WeaponBobLocalPosition + weaponRecoilLocalPosition;
        }
        #endregion

        #region Custom Method
        //ī�޶� FOV ����
        private void SetFov(float fov)
        {
            playerCharacterController.PlayerCamera.fieldOfView = fov;
            weaponCamera.fieldOfView = fov * weaponFovMultiplier;
        }

        //���� ���⿡ ���� ���� ��ġ ����
        private void UpdateWeaponAiming()
        {
            if (weaponSwitchState != WeaponSwitchState.Up)
                return;

            WeaponController activeWeapon = GetActiveWeapon();

            if (IsAiming && activeWeapon)   //����
            {
                if (isScopeOn)
                {
                    //�Ÿ� ���� �� isScopeOn = false
                    float distance = Vector3.Distance(weaponMainLocalPosition, aimingWeaponPosition.localPosition + activeWeapon.aimOffset);
                    if (distance < 0.05f)
                    {
                        //���� ��� ����
                        isScopeOn = false;

                        //ScopeUI Ȱ��ȭ, ���Ⱑ �Ⱥ��̵��� �����
                        OnScopeWeapon?.Invoke();
                    }
                }

                //��ġ ����
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition, aimingWeaponPosition.localPosition + activeWeapon.aimOffset, aimingAnimationSpeed * Time.deltaTime);
                //Fov ����
                if(isScopeOn == false)
                {
                    aimingFov = Mathf.Lerp(playerCharacterController.PlayerCamera.fieldOfView, defaultFov * activeWeapon.aimZoomRatio, aimingAnimationSpeed * Time.deltaTime);
                    SetFov(aimingFov);
                }
               
            }
            else  //����X
            {
                //��ġ ����
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition, defaultWeaponPosition.localPosition, aimingAnimationSpeed * Time.deltaTime);
                //Fov ����
                aimingFov = Mathf.Lerp(playerCharacterController.PlayerCamera.fieldOfView, defaultFov, aimingAnimationSpeed * Time.deltaTime);
                SetFov(aimingFov);
            }
        }

        //�ݵ� ���⿡ ���� ���Ⱑ �ڷ� �и��� ���ϱ�
        private void UpdateWeaponRecoil()
        {
            //accumulateRecoil: ���� ���� �ڷ� �и� ��
            //weaponRecoilLocalPosition: �ڷ� �и� ��
            if(weaponRecoilLocalPosition.z >= accumulateRecoil.z * 0.99f)   //�ڷ� �и��� ����
            {
                weaponRecoilLocalPosition = Vector3.Lerp(weaponRecoilLocalPosition, accumulateRecoil, recoilSharpness * Time.deltaTime);
            }
            else
            {
                weaponRecoilLocalPosition = Vector3.Lerp(weaponRecoilLocalPosition, Vector3.zero, recoilRepositionSharpness * Time.deltaTime);
                accumulateRecoil = weaponRecoilLocalPosition;
            }
        }

        //��鸲�� ���� ���� ��鸲 �� ���ϱ�
        private void UpdateWeaponBob()
        {
            if(Time.deltaTime > 0)
            {
                //�̹� �����ӿ� �̵��� �Ÿ�
                Vector3 playerCharacterVelocity = (playerCharacterController.transform.position - m_LastCharacterPosition) / Time.deltaTime;

                //���ӿ��� ĳ������ �̵� �ӵ� ���(0~1) ���� : 0 , Max �̵��ӵ� �� 1
                float charactorMovementFactor = 0f;
                if (playerCharacterController.IsGrounded)
                {
                    charactorMovementFactor = Mathf.Clamp01(playerCharacterVelocity.magnitude / 
                        (playerCharacterController.MaxSpeedOnGround * playerCharacterController.SprintSpeedModifier));
                }

                m_WeaponBobFactor = Mathf.Lerp(m_WeaponBobFactor, charactorMovementFactor, bobSharpness * Time.deltaTime);

                //BobFactor�� ���� ��鸲 ��, ��鸲 �ӵ��� �������� m_WeaponBobLocalPosition  ���ϱ�
                float bobAmount = IsAiming ? aimingBobAmount : defaultBobAmount;
                float frequency = bobFrequency;
                //�� �� �̵���
                float hBobValue = Mathf.Sin(Time.time * frequency) * bobAmount * m_WeaponBobFactor;
                //�� �Ʒ� �̵���
                float vBobValue = (Mathf.Sin(Time.time * frequency * 2) * 0.5f + 0.5f) * bobAmount * m_WeaponBobFactor;

                //��鸲 ���� ��ġ
                m_WeaponBobLocalPosition.x = hBobValue;
                m_WeaponBobLocalPosition.y = vBobValue;
                //
                m_LastCharacterPosition = playerCharacterController.transform.position;
            }
        }

        //���� ��ü ���� �� ���� ���� ����
        private void UpdateWeaponState()
        {
            //Lerp t����
            float switchingTimeFactor = 0f;
            if (weaponSwitchDelay <= 0f)
            {
                switchingTimeFactor = 1f;
            }
            else
            {
                switchingTimeFactor = Mathf.Clamp01((Time.time - weaponSwitchTimeStarted) / weaponSwitchDelay);
            }

            //Ÿ�̸Ӱ� �Ϸ�Ǿ����� ���� �Ϸ��ϰ� ���� ����
            if(switchingTimeFactor >= 1f)
            {
                //����Ʈ ��ġ���� �Ʒ� ��ġ�� �̵� �Ϸ��� ����
                if (weaponSwitchState == WeaponSwitchState.PutDownPrevious)
                {
                    //���� ��ü : ���� ���� false,���ο� ���� true
                    WeaponController oldWeapon = GetWeaponSlotIndex(ActiveWeaponIndex);
                    if(oldWeapon != null)
                    {
                        oldWeapon.ShowWeapon(false);
                    }
                    //���ο� ���� �ε����� ��Ƽ�� �ε����� ����
                    ActiveWeaponIndex = weaponSwitchNewWeaponIndex;

                    //��Ƽ�� �ε��� �ش�Ǵ� ����(weaponController)��������
                    WeaponController newWeapon = GetWeaponSlotIndex(ActiveWeaponIndex);
                    //��Ƽ�� ����(weaponController)�� �Ű������� �� ��ϵ� �Լ��� ȣ��
                    OnSwitchToWeapon?.Invoke(newWeapon);

                    switchingTimeFactor = 0f;
                    if(newWeapon != null)   //���ο� ���Ⱑ ������ ���� ����
                    {
                        weaponSwitchTimeStarted = Time.time;
                        weaponSwitchState = WeaponSwitchState.PutUpNew;
                    }
                    else    //���ο� ���Ⱑ ���� ��
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
            else    //0 -> 1 ���� ��
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

        //�Ű������� ���� ����(Weapon Prefab)�� ���� ����Ʈ�� �߰�
        private bool AddWeapon(WeaponController weaponPrefab)
        {
            //���� �߰��ϴ� ���� ���� ���� - �ߺ� �˻�
            if(HasWeapon(weaponPrefab) != null)
            {
                Debug.Log("Has Same Weapon");
                return false;
            }

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                //�� ���� ã��
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

        //�Ű������� ���� ���������� ������ ���Ⱑ ������ ������ ���⸦ ��ȯ
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

        //���� ��Ƽ���� ���� ��������
        public WeaponController GetActiveWeapon()
        {
            return GetWeaponSlotIndex(ActiveWeaponIndex);
        }

        //���� �ε����� ���� ��������
        private WeaponController GetWeaponSlotIndex(int index)
        {
            if (index < 0 || index >= weaponSlots.Length)
                return null;

            return weaponSlots[index];
        }

        //���� ��� �ִ� ���� false, ���ο� ���� true;
        //ascendingOrder ���� ���� �������� ���� : �ε����� ��������, ��������
        private void SwitchWeapon(bool ascendingOrder)
        {
            //���ο� ������ �ε���
            int newWeaponIndex = -1;
            //���� Ȱ��ȭ�� ������ ���� ����� ���� ã��
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

            //���ο� ������ �ε����� ���� ��ü
            SwitchToWeaponIndex(newWeaponIndex);
        }

        //�Ű������� ���� ����� ��ü
        private void SwitchToWeaponIndex(int newWeaponIndex)
        {
            if (newWeaponIndex == ActiveWeaponIndex)
                return;

            if (newWeaponIndex < 0 || newWeaponIndex >= weaponSlots.Length)
                return;

            weaponSwitchNewWeaponIndex = newWeaponIndex;
            //������� �ð� ����
            weaponSwitchTimeStarted = Time.time;

            if(GetActiveWeapon() == null)
            {
                //���� ��ġ�� �Ʒ� ��ġ�� ������ ���´�
                weaponMainLocalPosition = downWeaponPosition.localPosition;
                //�ø��� ���·� ����
                weaponSwitchState = WeaponSwitchState.PutUpNew;
                //���ο� ���� �ε����� ��Ƽ�� �ε����� ����
                ActiveWeaponIndex = newWeaponIndex;

                //��Ƽ�� �ε��� �ش�Ǵ� ����(weaponController)��������
                WeaponController weaponController = GetWeaponSlotIndex(ActiveWeaponIndex);
                //��Ƽ�� ����(weaponController)�� �Ű������� �� ��ϵ� �Լ��� ȣ��
                OnSwitchToWeapon?.Invoke(weaponController);
            }
            else
            {
                weaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }

        //���� ���԰��� �Ÿ� ���ϱ�
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

        //�Ű������� ���� ����� ��ü
        private void OnWeaponSwitched(WeaponController newWeapon)
        {
            if(newWeapon != null)
            {
                newWeapon.ShowWeapon(true);
            }
        }

        //���� ���� ī�޶� On
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
