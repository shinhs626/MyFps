using UnityEngine;
using TMPro;
using MyDefence;

namespace MyFps
{
    //문열기 인터렉티브 액션 구현
    public class DoorCellOpen : Interactive
    {
        #region Variables
        //페이더
        private SceneFader fader;

        //애니메이션
        public Animator animator;

        //애니 파라미터 스트링
        private string paramIsOpen = "IsOpen";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //fader.FadeStart(3f);
            fader.FadeTo("PlayScene");
        }
        private void OnMouseOver()
        {
            if(theDistance <= 2)
            {
                crossHairUI.SetActive(true);
                ShowActionUI();

                //ToDo : New Input System 구현
                //키 입력 체크
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //UI를 숨기고 문열고
                    HideActionUI();
                    animator.SetBool(paramIsOpen, true);
                    this.GetComponent<BoxCollider>().enabled = false;
                }
            }
            else
            {
                HideActionUI();
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
