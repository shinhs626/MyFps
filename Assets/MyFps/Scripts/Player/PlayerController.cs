using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //����
        private CharacterController controller;
        public GameObject damageEffect;
        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;

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

        //ü��
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //���� ó��
        public SceneFader fader;
        [SerializeField]private string loadToScene = "GameOverScene";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            controller = this.GetComponent<CharacterController>();

            currentHealth = maxHealth;
        }

        private void Update()
        {
            //���� ������
            bool isGrounded = GroundCheck();
            if (isGrounded && velocity.y < 0f)
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
            if (context.started && GroundCheck())
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

        //�÷��̾� ������
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"currentHealth: {currentHealth}");

            //������ ���� Sfx, Vfx
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

            //���� ó��
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
