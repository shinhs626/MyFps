using TMPro;
using UnityEngine;

namespace MyFps
{
    public class PickupPistol : Interactive
    {
        #region Variables
        public GameObject realPistol;
        public GameObject theArrow;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //무기획득, 충돌체 제거
            realPistol.SetActive(true);
            theArrow.SetActive(false);
            this.gameObject.SetActive(false);   //fake pistol 및 충돌체 제거  
        }
        #endregion
    }

}