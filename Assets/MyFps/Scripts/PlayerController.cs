using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        private CharacterController controller;

        //입력
        [SerializeField] private Vector2 inputVector;

        //이동
        [SerializeField] private float moveSpeed = 10f;

        //중력
        private float gravity = -9.81f;
        [SerializeField]
        private Vector3 velocity;   //중력

        //그라운드 체크
        public Transform groundCheck;   //발 바닥 위치
        [SerializeField] private float checkRange = 0.2f;   //체크하는 구의 반경
        [SerializeField] private LayerMask groundMask;  //그라운드 레이어 판별

        //점프 높이
        [SerializeField] private float jumpHeight = 1f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            controller = this.GetComponent<CharacterController>();
        }

        private void Update()
        {
            //땅에 있으면
            bool isGrounded = GroundCheck();
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -5f;
            }

            //방향
            //Global
            //Vector3 moveDir = Vector3.right * inputVector.x + Vector3.forward * inputVector.y;  //앞

            //Local
            Vector3 moveDir = transform.right * inputVector.x + transform.forward * inputVector.y;

            //Vector3 moveDir = Vector3.forward * -1;   //뒤
            //Vector3 moveDir = Vector3.right;  //우
            //Vector3 moveDir = Vector3.right * -1; //좌

            controller.Move(moveDir * Time.deltaTime * moveSpeed);

            //중력에 따른 y축 이동
            velocity.y += 2f * gravity * Time.deltaTime;
            controller.Move(velocity);
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            inputVector = context.ReadValue<Vector2>();
            //Debug.Log($"inputMove:{inputVector}");
        }
        bool GroundCheck()
        {
            return Physics.CheckSphere(groundCheck.position, checkRange, groundMask);
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && GroundCheck())
            {
                //점프 높이
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }
        #endregion
    }

}
