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

        #region Unity Event Method
        private void OnMouseOver()
        {
            if(theDistance <= 2)
            {
                crossHairUI.SetActive(true);
                ShowActionUI();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HideActionUI();
                    this.gameObject.SetActive(false);
                    realPistol.SetActive(true);
                    theArrow.SetActive(false);
                    //Debug.Log("Pick Up Pistol");
                }
            }
        }
        private void OnMouseExit()
        {
            crossHairUI.SetActive(false);
            HideActionUI();
        }
        #endregion
    }

}