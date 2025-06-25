using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class WeaponHUDManager : MonoBehaviour
    {
        #region Variables
        //����
        private PlayerWeaponManager playerWeaponManager;

        public RectTransform ammoPanel;     //ammoCount ������ ������Ʈ�� �θ� ������Ʈ
        public AmmoCounter ammoCounterPrefab;

        //ammoCounter UI ����Ʈ
        private List<AmmoCounter> ammoCounters = new List<AmmoCounter>();
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //����
            playerWeaponManager = FindFirstObjectByType<PlayerWeaponManager>();
        }
        private void Start()
        {
            WeaponController activeWeapon = playerWeaponManager.GetActiveWeapon();
            if (activeWeapon)
            {
                AddWeapon(activeWeapon, playerWeaponManager.ActiveWeaponIndex);
                SwitchWeapon(activeWeapon);
            }

            //�ʱ�ȭ
            playerWeaponManager.OnAddedWeapon += AddWeapon;
            playerWeaponManager.OnRemovedWeapon += RemoveWeapon;
            playerWeaponManager.OnSwitchToWeapon += SwitchWeapon;

        }
        #endregion

        #region Custom Method
        //���� �߰��� UI ������ �߰�
        private void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            AmmoCounter ammoCounter = Instantiate(ammoCounterPrefab, ammoPanel);
            //UI �ʱ�ȭ
            ammoCounter.Initialize(newWeapon, weaponIndex);

            //UI ����Ʈ�� �߰�
            ammoCounters.Add(ammoCounter);
        }

        //���� ���Ž� UI ������Ʈ ����
        private void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {
            //������ AmmoCounter UI ã��
            int findCounterIndex = -1;
            for (int i = 0; i < ammoCounters.Count; i++)
            {
                if (ammoCounters[i].WeaponCounterIndex == weaponIndex)
                {
                    findCounterIndex = i;
                    //UI ������Ʈ ų
                    Destroy(ammoCounters[i].gameObject);
                    break;
                }
            }
            //UI�� ã�Ҵٸ� ����Ʈ������ ����
            if(findCounterIndex >= 0)
            {
                ammoCounters.RemoveAt(findCounterIndex);
            }

        }

        //���� ��ü�� (ammoPanel) UI ������(����)
        private void SwitchWeapon(WeaponController weapon)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ammoPanel);
        }
        #endregion
    }

}
