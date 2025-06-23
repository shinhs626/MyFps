using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class ScopeUIManager : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager weaponManager;

        public GameObject scopeUI;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            weaponManager = FindFirstObjectByType<PlayerWeaponManager>();
        }
        private void OnEnable()
        {
            weaponManager.OnScopeWeapon += OnScopeUI;
            weaponManager.OffScopeWeapon += OffScopeUI;
        }
        private void OnDisable()
        {
            weaponManager.OnScopeWeapon -= OnScopeUI;
            weaponManager.OffScopeWeapon -= OffScopeUI;
        }
        #endregion

        #region Custom Method
        private void OnScopeUI()
        {
            scopeUI.SetActive(true);
        }
        private void OffScopeUI()
        {
            scopeUI.SetActive(false);
        }
        #endregion
    }
}

