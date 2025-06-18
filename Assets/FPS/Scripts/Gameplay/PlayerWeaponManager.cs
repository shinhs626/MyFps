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

        //���� ��ü ����
        private WeaponSwitchState weaponSwitchState;

        //���� ��ü�� ������ �ε���
        private int weaponSwitchNewWeaponIndex;

        private float weaponSwitchTimeStarted = 0f;     //���� ���� �ð�
        private float weaponSwitchDelay = 1f;           //���� �÷��� �ð�

        //�� ����
        public Camera weaponCamera;
        #endregion

        #region Property
        //���� ����Ʈ(weaponSlots)�� �����ϴ� �ε��� - ���� ��Ƽ���� ������ �ε���
        public int ActiveWeaponIndex { get; private set; }

        //�� ���� üũ
        public bool IsPointingAtEnemy { get; private set; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            inputHandler = this.GetComponent<PlayerInputHandler>();

            //�ʱ�ȭ
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;

            //���� ��ü�� ȣ��� �Լ� ���
            OnSwitchToWeapon += OnWeaponSwitched;

            //ó�� ���� ���� ���⸦ �����Ѵ�
            foreach(var w in startingWeapons)
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
            //���� ��ü ����
            UpdateWeaponState();

            //������ ���� ��ġ
            weaponParentSocket.localPosition = weaponMainLocalPosition;
        }
        #endregion

        #region Custom Method
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
        #endregion
    }

}
