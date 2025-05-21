using UnityEngine;

namespace MySample
{
    public class MoveObejct : MonoBehaviour
    {
        #region Variables
        //����
        private Rigidbody rb;

        //������Ʈ�� �ִ� ��
        [SerializeField]
        private float movePower = 3f;

        //������Ʈ �÷� �ٲٱ�
        private Renderer render;
        private Color originColor;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            rb = this.GetComponent<Rigidbody>();
            render = this.GetComponent<Renderer>();

            //�ʱ�ȭ
            originColor = render.material.color;

            //�����Ҷ� ������ �� �ش�: 3f  - ��ȸ�� ��
            MoveRight();
        }

        #endregion

        #region Custom Method
        //�÷��̾ �������� 3��ŭ ���� �ش�
        public void MoveLeft()
        {
            rb.AddForce(Vector3.left * movePower, ForceMode.Impulse);
        }

        //�÷��̾ ���������� 3��ŭ ���� �ش�
        public void MoveRight()
        {
            rb.AddForce(Vector3.right * movePower, ForceMode.Impulse);
        }

        //���������� �ٲ۴�
        public void ChangeColorRed()
        {
            render.material.color = Color.red;
        }

        //�������� �÷��� �ٲ۴�
        public void ChangeColorOrigin()
        {
            render.material.color = originColor;
        }
        #endregion
    }
}