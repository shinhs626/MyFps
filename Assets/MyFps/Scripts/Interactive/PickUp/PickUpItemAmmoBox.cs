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
        //������ üũ�Ͽ� �������� ������ true, �� ������ false
        protected override bool OnPickUp()
        {
            PlayerDataManager.Instance.AddAmmo(addAmmo);
            return true;
        }
        #endregion
    }

}
