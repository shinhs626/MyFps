using UnityEngine;
using TMPro;

namespace MyFps
{
    //������ ���ͷ�Ƽ�� �׼� ����
    public class DoorCellOpen : Interactive
    {
        #region Variables        
        //�ִϸ��̼�
        public Animator animator;
        //�ִ� �Ķ���� ��Ʈ��
        private string  paramIsOpen = "IsOpen";

        //������ �Ҹ�
        public AudioSource audioSource;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //������, �浹ü ����
            animator.SetBool(paramIsOpen, true);        //�� ���� �ִϸ��̼� ����

            //������ �Ҹ�
            audioSource.Play();

            this.GetComponent<BoxCollider>().enabled = false; //�� �浹ü ����
        }
        #endregion
    }
}
