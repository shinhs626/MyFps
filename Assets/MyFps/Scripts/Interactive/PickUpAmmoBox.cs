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
            Debug.Log("źȯ 7���� ȹ���ߴ�.");
            KeepAmmo(ammo);

            //����Ʈ Vfx, Sfx

            //Ʈ���� ����, ų
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
