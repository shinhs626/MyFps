using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        private CharacterController controller;
        public GameObject damageEffect;
        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;

        //입력 - 이동
        private Vector2 inputMove;

        //이동
        [SerializeField]
        private float moveSpeed = 10f;

        //중력
        private float gravity = -9.81f;
        [SerializeField]
        private Vector3 velocity;       //중력 계산에 의한 이동 속도

        //그라운드 체크
        public Transform groundCheck;   //발 바닥 위치
        [SerializeField] private float checkRange = 0.2f;    //체크 하는 구의 반경
        [SerializeField] private LayerMask groundMask;       //그라운드 레이어 판별

        //점프 높이
        [SerializeField] private float jumpHeight = 1f;

        //체력
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //죽음 처리
        public SceneFader fader;
        [SerializeField]private string loadToScene = "GameOverScene";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            controller = this.GetComponent<CharacterController>();

            currentHealth = maxHealth;
        }

        private void Update()
        {
            //땅에 있으면
            bool isGrounded = GroundCheck();
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -10f;
            }

            //방향
            //Global축 이동
            //Vector3 moveDir = Vector3.right * inputMove.x + Vector3.forward * inputMove.y;
            //Local축 이동
            Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

            //이동
            controller.Move(moveDir * Time.deltaTime * moveSpeed);

            //중력에 따른 y축 이동
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }
        #endregion

        #region Custom Method
        //Input 시스템에 등록할 함수
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && GroundCheck())
            {
                //점프 높이만큼 뛰기 위한 속도 구하기
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }

        //그라운드 체크
        bool GroundCheck()
        {
            return Physics.CheckSphere(groundCheck.position, checkRange, groundMask);
        }

        //플레이어 데미지
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"currentHealth: {currentHealth}");

            //데미지 연출 Sfx, Vfx
            StartCoroutine(DamageEffect());

            if(currentHealth <= 0 && isDeath == false)
            {
                Die();
            }
        }
        IEnumerator DamageEffect()
        {
            damageEffect.SetActive(true);
            PlayRandomHurt();
            yield return new WaitForSeconds(1f);
            damageEffect.SetActive(false);
        }
        private void Die()
        {
            isDeath = true;

            //죽음 처리
            fader.FadeTo(loadToScene);
        }
        private void PlayRandomHurt()
        {
            int rand = Random.Range(0, 3);

            switch (rand)
            {
                case 0:
                    hurt01.Play();
                    break;
                case 1:
                    hurt02.Play();
                    break;
                case 2:
                    hurt03.Play();
                    break;
            }
        }
        #endregion
    }
}
