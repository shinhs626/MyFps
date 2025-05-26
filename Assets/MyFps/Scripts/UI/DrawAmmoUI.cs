using TMPro;
using UnityEngine;

namespace MyFps
{
    //Ammo ���� UI�� ����
    public class DrawAmmoUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI ammoCountText;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //Ammo ������ UI�� ����
            ammoCountText.text = PlayerDataManager.Instance.AmmoCount.ToString();
        }
        #endregion
    }

}
