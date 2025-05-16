using UnityEngine;
using TMPro;

namespace MyFps
{
    //문열기 인터렉티브 액션 구현
    public class DoorCellOpen : MonoBehaviour
    {
        #region Variables
        //문과 플레이어와의 거리
        private float theDistance;

        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        [SerializeField]public string action = "Open The Door";

        //애니메이션
        public Animator animator;

        //애니 파라미터 스트링
        private string paramIsOpen = "IsOpen";
        #endregion

        #region Unity Event Method
        private void Update()
        {
            theDistance = PlayerCasting.distanceFromTarget;
        }
        private void OnMouseOver()
        {
            if(theDistance <= 2)
            {
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
            HideActionUI();
        }
        #endregion
        #region Custom Method
        private void ShowActionUI()
        {
            actionUI.SetActive(true);
            actionText.text = action;
        }
        private void HideActionUI()
        {
            actionUI.SetActive(false);
            actionText.text = " ";
        }
        #endregion
    }

}
