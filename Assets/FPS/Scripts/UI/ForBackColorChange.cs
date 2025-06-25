using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    //UI ������ Bar �̹��� �÷� ���� ȿ��
    public class ForBackColorChange : MonoBehaviour
    {
        #region Variables
        public Image forgroundImage;
        public Color defaultForgroundColor;     //�������� �⺻ �÷�
        public Color fullFlashForGroundColor;   //100% ������ �÷��� ȿ�� �÷�

        public Image backgroundImage;           //
        public Color defaultBackgroundColor;    //�������� ��׶��� �̹��� �⺻ �÷�
        public Color emptyFlashBackGroundColor; //0%�� �÷��� ȿ�� �÷�

        [SerializeField] private float fullValue = 1f;       //�������� rate Max Value
        [SerializeField] private float emptyValue = 0f;      //�������� rate Min Value

        [SerializeField]
        private float colorChangeSharpness = 5f;        //�÷� ���� �ӵ� (Lerp)

        private float m_PreviousValue;      //������ ���� was ����
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        //UI ������ Bar �ʱ�ȭ
        public void Initialized(float fullValueRatio, float emptyValueRatio)
        {
            fullValue = fullValueRatio;
            emptyValue = emptyValueRatio;

            m_PreviousValue = fullValue;
        }

        //������ Bar ȿ�� Update
        public void UpdateVisual(float currentRatio)
        {
            //100% �����Ǵ� ����
            if(currentRatio == fullValue && currentRatio != m_PreviousValue)
            {
                forgroundImage.color = fullFlashForGroundColor;
            }
            else if (currentRatio < emptyValue)
            {
                backgroundImage.color = emptyFlashBackGroundColor;
            }
            else
            {
                forgroundImage.color = Color.Lerp(forgroundImage.color, defaultForgroundColor, Time.deltaTime * colorChangeSharpness);
                backgroundImage.color = Color.Lerp(backgroundImage.color, defaultBackgroundColor, Time.deltaTime * colorChangeSharpness);
            }

            //
            m_PreviousValue = currentRatio;
        }
        #endregion
    }

}
