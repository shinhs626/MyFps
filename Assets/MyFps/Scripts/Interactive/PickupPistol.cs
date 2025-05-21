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
        #endregion

        #region Custom Method  
        protected override void DoAction()
        {
            //����ȹ��, �浹ü ����
            realPistol.SetActive(true);
            theArrow.SetActive(false);

            this.gameObject.SetActive(false);   //fake pistol �� �浹ü ����                    
        }
        #endregion
    }
}
