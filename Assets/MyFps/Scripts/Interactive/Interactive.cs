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
        #endregion
    }

}
