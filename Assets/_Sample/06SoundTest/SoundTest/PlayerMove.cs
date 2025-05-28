using UnityEngine;

namespace MySample
{
    public class PlayerMove : MonoBehaviour
    {
        #region Variables
        //����
        private Rigidbody rb;

        //�� ���� �̵��ϴ� ��
        [SerializeField]
        private float forwardForce = 10f;
        //�¿�� �̵��ϴ� ��
        [SerializeField]
        private float sideForce = 5f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            //rb.AddForce(Vector3.forward * forwardForce);
            rb.AddForce(0f, 0f, forwardForce, ForceMode.Acceleration);
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.left * sideForce);
            }
            if(Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.right * sideForce);
            }
        }
        #endregion
    }

}
