using UnityEngine;

namespace MyFps
{
    public class PickUpItemAmmoBox : PickUpItem
    {
        #region Variables
        [SerializeField]
        private int addAmmo = 7;
        #endregion

        #region Unity Event Method

        #endregion
        #region Custom Method
        //조건을 체크하여 아이템을 먹으면 true, 못 먹으면 false
        protected override bool OnPickUp()
        {
            PlayerDataManager.Instance.AddAmmo(addAmmo);
            return true;
        }
        #endregion
    }

}
