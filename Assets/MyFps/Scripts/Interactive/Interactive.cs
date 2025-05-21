using TMPro;
using UnityEngine;

namespace MyFps
{
    //���ͷ�Ƽ�� �׼��� �θ� Ŭ����
    public class Interactive : MonoBehaviour
    {
        #region Variables
        //���� �÷��̾���� �Ÿ�
        protected float theDistance;

        //�׼� UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        //ũ�ν����
        public GameObject extraCross;

        [SerializeField]
        protected string action = "Do Interactive Action";
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //���� �÷��̾���� �Ÿ� ��������
            theDistance = PlayerCasting.distanceFromTarget;
        }

        private void OnMouseOver()
        {
            extraCross.SetActive(true);

            if (theDistance <= 2f)
            {
                ShowActionUI();

                //TODO : New Input System ��ü ����
                //Ű�Է� üũ
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //
                    extraCross.SetActive(false);

                    //UI �����
                    HideActionUI();

                    //�׼�
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
            extraCross.SetActive(false);
            HideActionUI();
        }
        #endregion

        #region Custom Method
        //Action UI �����ֱ�
        protected void ShowActionUI()
        {
            actionUI.SetActive(true);
            actionText.text = action;
        }

        //Action UI �����
        protected void HideActionUI()
        {
            actionUI.SetActive(false);
            actionText.text = "";
        }

        //�׼� �Լ�
        protected virtual void DoAction()
        {

        }
        #endregion
    }
}
