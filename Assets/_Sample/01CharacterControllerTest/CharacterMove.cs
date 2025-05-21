using UnityEngine;
using UnityEngine.InputSystem;

namespace MySample
{
    //ĳ���� �̵��� ���� Ŭ����
    public class CharacterMove : MonoBehaviour
    {
        #region Variables
        //����
        private CharacterController controller;

        //�Է� - �̵�
        private Vector2 inputMove;

        //�̵�
        [SerializeField]
        private float moveSpeed = 10f;

        //�߷�
        private float gravity = -9.81f;
        [SerializeField]
        private Vector3 velocity;       //�߷� ��꿡 ���� �̵� �ӵ�

        //�׶��� üũ
        public Transform groundCheck;   //�� �ٴ� ��ġ
        [SerializeField] private float checkRange = 0.2f;    //üũ �ϴ� ���� �ݰ�
        [SerializeField] private LayerMask groundMask;       //�׶��� ���̾� �Ǻ�

        //���� ����
        [SerializeField] private float jumpHeight = 1f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            controller = this.GetComponent<CharacterController>();
        }

        private void Update()
        {
            //���� ������
            bool isGrounded = GroundCheck();
            if(isGrounded && velocity.y < 0f)
            {
                velocity.y = -10f;
            }

            //����
            //Global�� �̵�
            //Vector3 moveDir = Vector3.right * inputMove.x + Vector3.forward * inputMove.y;
            //Local�� �̵�
            Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

            //�̵�
            controller.Move(moveDir * Time.deltaTime * moveSpeed);

            //�߷¿� ���� y�� �̵�
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }
        #endregion

        #region Custom Method
        //Input �ý��ۿ� ����� �Լ�
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.started && GroundCheck())
            {
                //���� ���̸�ŭ �ٱ� ���� �ӵ� ���ϱ�
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }

        //�׶��� üũ
        bool GroundCheck()
        {
            return Physics.CheckSphere(groundCheck.position, checkRange, groundMask);
        }
        #endregion
    }
}


/*
-. �ӵ� = �߷°��ӵ� x �ð�                  [ v = gt ]
-. ������ �Ÿ� = �� x �߷°��ӵ� x �ð� ����   [ h = �� gt��]
*/