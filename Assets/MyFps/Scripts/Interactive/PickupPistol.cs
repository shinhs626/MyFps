using TMPro;
using UnityEngine;

namespace MyFps
{
    //������(����) ȹ�� ���ͷ�Ƽ�� ����
    public class PickupPistol : Interactive
    {
        #region Variables        
        //���ͷ�Ƽ�� �׼� ����
        public GameObject realPistol;
        public GameObject theArrow;
        public GameObject ammoUI;
        public GameObject ammoBox;
        public GameObject secondTrigger;
        #endregion

        #region Custom Method  
        protected override void DoAction()
        {
            //����ȹ��, �浹ü ����
            realPistol.SetActive(true);
            ammoUI.SetActive(true);
            ammoBox.SetActive(true);
            secondTrigger.SetActive(true);
            //���� ������ ����
            PlayerDataManager.Instance.Weapon = WeaponType.Pistol;
            theArrow.SetActive(false);

            this.gameObject.SetActive(false);   //fake pistol �� �浹ü ����                    
        }
        #endregion
    }
}
