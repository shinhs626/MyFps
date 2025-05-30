using System.Collections;
using TMPro;
using UnityEngine;

namespace MyFps
{
    //��� Ű�� ������
    public class DoorKeyOpen : Interactive
    {
        #region Variables
        //����
        public Animator animator;

        //�ó����� �ؽ�Ʈ
        public TextMeshProUGUI sequenceText;
        [SerializeField]
        private string sequence = "You need to the Key";
        #endregion

        #region Unity Event Method
        
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //���ǿ� ���� ���� �ȿ�����
            if (PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.ROOM01_KEY))
            {
                //������
                OpenedDoor();
            }
            else
            {
                //�� �ȿ���
                StartCoroutine(LockedDoor());
            }
            
        }

        private void OpenedDoor()
        {
            this.GetComponent<BoxCollider>().enabled = false;

            AudioManager.Instance.Play("DoorBang");

            animator.SetBool("IsOpen", true);
        }

        IEnumerator LockedDoor()
        {
            unInteractive = true;

            sequenceText.text = "";

            yield return new WaitForSeconds(1f);

            sequenceText.text = sequence;

            yield return new WaitForSeconds(2f);

            sequenceText.text = "";

            unInteractive = false;
        }
        #endregion
    }

}
