using System.Collections;
using TMPro;
using UnityEngine;

namespace MyFps
{
    //퍼즈ㄹ 키로 문열기
    public class DoorKeyOpen : Interactive
    {
        #region Variables
        //참조
        public Animator animator;

        //시나리오 텍스트
        public TextMeshProUGUI sequenceText;
        [SerializeField]
        private string sequence = "You need to the Key";
        #endregion

        #region Unity Event Method
        
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //조건에 따라 문이 안열린다
            if (PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.ROOM01_KEY))
            {
                //문열기
                OpenedDoor();
            }
            else
            {
                //문 안열림
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
