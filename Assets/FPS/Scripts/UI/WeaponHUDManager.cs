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
            //�ʱ�ȭ
            playerWeaponManager.OnAddedWeapon += AddWeapon;
            playerWeaponManager.OnRemovedWeapon += RemoveWeapon;

        }
        #endregion

        #region Custom Method
        //���� �߰��� UI ������ �߰�
        private void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            AmmoCounter ammoCounter = Instantiate(ammoCounterPrefab, ammoPanel);

            ammoCounters.Add(ammoCounter);
        }

        //���� ���Ž� UI ������Ʈ ����
        private void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {

        }
        #endregion
    }

}
