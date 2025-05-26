using UnityEngine;

namespace MyFps
{
    public class PickUpAmmoBox : Interactive
    {
        #region Variables
        [SerializeField]
        private int ammo = 7;
        #endregion

        #region Custom Method  
        protected override void DoAction()
        {
            Debug.Log("≈∫»Ø 7∞≥∏¶ »πµÊ«ﬂ¥Ÿ.");
            KeepAmmo(ammo);

            //¿Ã∆Â∆Æ Vfx, Sfx

            //∆Æ∏Æ∞≈ ¡¶∞≈, ≈≥
            this.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        public void KeepAmmo(int amount)
        {
            PlayerDataManager.Instance.AddAmmo(amount);
        }
        #endregion
    }

}
