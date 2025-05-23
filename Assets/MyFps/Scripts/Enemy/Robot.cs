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

        private RobotHealth robotHealth;

        //�κ��� ���� ����
        private RobotState robotState;
        //�κ��� ���� ����
        private RobotState beforeState;

        //�̵�
        [SerializeField]
        private float moveSpeed = 5f;

        //����
        [SerializeField]
        private float attackRange = 1.5f;

        //�ִϸ��̼� �Ķ����
        private string enemyState = "EnemyState";

        //���ݷ�
        [SerializeField]private float attackDamage = 5f;

        //���� Ÿ�̸�
        [SerializeField]
        private float attackTime = 2f;
        private float countdown;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //����
            animator = this.GetComponent<Animator>();
            robotHealth = this.GetComponent<RobotHealth>();
        }

        private void OnEnable()
        {
            //�̺�Ʈ �Լ� ���
            robotHealth.OnDie += OnDie;

            //�ʱ�ȭ
            ChangeState(RobotState.R_Idle);
        }
        private void OnDisable()
        {
            //�̺�Ʈ �Լ� ����
            robotHealth.OnDie -= OnDie;
        }

        private void Update()
        {
            if (robotHealth.IsDeath)
                return;

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
                    //2�ʸ��� �������� 5�� �ش�
                    OnAttackTimer();

                    //���� ���� üũ
                    if(distance > attackRange)
                    {
                        ChangeState(RobotState.R_Walk);
                    }
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
        private void OnAttackTimer()
        {
            countdown += Time.deltaTime;
            if (countdown >= attackTime)
            {
                //Ÿ�̸� ����
                Attack();

                countdown = 0f;
            }
        }
        public void Attack()
        {
            Debug.Log($"�÷��̾�� {attackDamage}�� �ش�");
            IDamageable damageable = thePlayer.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
        private void OnDie()
        {
            ChangeState(RobotState.R_Death);
            this.GetComponent<BoxCollider>().enabled = false;
        }
        #endregion
    }
}