using TMPro;
using UnityEngine;

namespace MyFps
{
    //인터렉티브 액션의 부모 클래스
    public class Interactive : MonoBehaviour
    {
        #region Variables
        //문과 플레이어와의 거리
        protected float theDistance;

        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        //크로스헤어
        public GameObject crossHairUI;

        [SerializeField] protected string action = "Do Action";
        #endregion

        #region Unity Event Method
        protected void Update()
        {
            theDistance = PlayerCasting.distanceFromTarget;
        }
        private void OnMouseOver()
        {
            crossHairUI.SetActive(true);

            if (theDistance <= 2f)
            {
                ShowActionUI();

                //TODO : New Input System 대체 구현
                //키입력 체크
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //UI 숨기기
                    HideActionUI();

                    //액션
                    DoAction();
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
        #region Custom Method
        protected void ShowActionUI()
        {
            actionUI.SetActive(true);

            actionText.text = action;
        }
        protected void HideActionUI()
        {
            actionUI.SetActive(false);

            actionText.text = " ";
        }
        protected virtual void DoAction()
        {

        }
        #endregion
    }

}
