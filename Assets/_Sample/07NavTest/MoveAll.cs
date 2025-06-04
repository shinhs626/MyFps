using UnityEngine;

namespace MySample
{
    public class MoveAll : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float moveSpeed = 1f;

        [SerializeField]
        private float moveTime = 1f;
        [SerializeField]
        private float countdown = 0f;

        //�̵� ���� : 1�̸� ������, -1�̸� ����
        [SerializeField]
        private float dir = 1f;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //Ÿ�̸� �ð����� ������ �ٲ㼭 �̵�
            countdown += Time.deltaTime;
            if(countdown >= moveTime)
            {
                //������ �ٲ۴�
                dir *= -1;

                //�ʱ�ȭ
                countdown = 0f;
            }

            transform.Translate((Vector3.right * dir) * Time.deltaTime * moveSpeed, Space.World);
        }
        #endregion
    }

}
