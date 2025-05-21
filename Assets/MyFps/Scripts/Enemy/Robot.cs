using UnityEngine;

namespace MyFps
{
    //�κ� ����
    public enum RobotState
    {
        R_Idle = 0,
        R_Walk,
        R_Attack,
        R_Death
    }

    //enemy(�κ�)�� �����ϴ� Ŭ����
    public class Robot : MonoBehaviour
    {
        #region Variables
        //����
        private Animator animator;
        public Transform thePlayer; //Ÿ��

        //�κ��� ���� ����
        private RobotState robotState;
        //�κ��� ���� ����
        private RobotState beforeState;

        //ü��
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //�̵�
        [SerializeField]
        private float moveSpeed = 5f;

        //����
        [SerializeField]
        private float attackRange = 1.5f;

        //�ִϸ��̼� �Ķ����
        private string enemyState = "EnemyState";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            animator = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            //�ʱ�ȭ
            ChangeState(RobotState.R_Idle);
        }

        private void Update()
        {
            //�̵�
            Vector3 target = new Vector3(thePlayer.position.x, 0f, thePlayer.position.z);
            Vector3 dir = target - this.transform.position;
            float distance = Vector3.Distance(transform.position, target);
            //��Ÿ� üũ
            if(distance <= attackRange)
            {
                ChangeState(RobotState.R_Attack);
            }            

            //���� ����
            switch(robotState)
            {
                case RobotState.R_Idle:
                    break;

                case RobotState.R_Walk:
                    transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);
                    transform.LookAt(target);
                    break;

                case RobotState.R_Attack:
                    break;

                case RobotState.R_Death:
                    break;
            }
        }
        #endregion

        #region Custom Method
        //���ο� ���¸� �Ű������� �޾� ���ο� ���·� ����
        public void ChangeState(RobotState newState)
        {
            //���� ���� üũ
            if(robotState == newState)
            {
                return;
            }

            //���� ���¸� ���� ���·� ����
            beforeState = robotState;
            //���ο� ���¸� ���� ���·� ����
            robotState = newState;

            //���� ���濡 ���� ���� ����
            animator.SetInteger(enemyState, (int)robotState);
        }

        //������ �Ա�
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            //������ ���� (Sfx, Vfx)

            if(currentHealth <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //�ױ�
        private void Die()
        {
            isDeath = true;

            //���� ó��
            ChangeState(RobotState.R_Death);

            //����ó��..
        }
        #endregion
    }
}