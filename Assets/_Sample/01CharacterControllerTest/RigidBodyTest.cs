using UnityEngine;

namespace MySample
{
    public class RigidBodyTest : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;

        //��
        [SerializeField]
        private float power = 100f;
        #endregion

        private void Start()
        {
            //����
            rb = this.GetComponent<Rigidbody>();

            //��ȸ������  ���� �� �������� 100�� ������ ������Ʈ�� �̵���Ų��
            //rb.AddForce(Vector3.forward * power, ForceMode.Impulse);

            //��ȸ������  ���� �� �������� 100�� ������ ������Ʈ�� �̵���Ų��
            rb.AddForce(transform.forward * power, ForceMode.Impulse);
            //rb.AddRelativeForce(Vector3.forward * power, ForceMode.Impulse);
        }

        private void FixedUpdate()
        {
            //rb.AddForce(Vector3.forward, ForceMode.Force);
        }
    }
}

/*
ForceMode.Force (����, ���� ���)
- �ٶ�,�ڸ��� ó�� ���������� �־����� ��

ForceMode.Acceleration (����, ���� ����)
- �߷�, ������ ������� ������ ������ �����Ҷ�

ForceMode.Impulse (�ҿ���(1ȸ��), ���� ���)
- Ÿ��, ���� �� ���������� �����ϴ� ��

ForceMode.VelocityChange (�ҿ���(1ȸ��), ���� ����)
- ���������� ������ �ӵ��� ��ȭ�� �ٶ�
*/