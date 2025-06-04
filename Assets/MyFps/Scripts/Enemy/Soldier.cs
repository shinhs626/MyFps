using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    public class Soldier : MonoBehaviour
    {
        #region Variables
        //����
        private Animator animator;
        public Transform thePlayer; //Ÿ��
        private NavMeshAgent agent;

        private RobotHealth soldierHealth;

        private Vector3 target;

        //�κ��� ���� ����
        private RobotState soldierState;
        //�κ��� ���� ����
        private RobotState beforeState;

        //�ִϸ��̼� �Ķ����
        private string enemyState = "EnemyState";

        //����
        public Transform[] wayPoints;
        private int nowPointIndex = 0;

        //��� Ÿ�̸�
        [SerializeField] private float idleTime = 2f;
        private float idleCountdown = 0f;

        [SerializeField]
        private float detectingRange = 10f;

        [SerializeField]
        private float attackRange = 4f;

        [SerializeField]
        private float attackTimer = 2f;
        private float attackCountdown = 0f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            animator = this.GetComponent<Animator>();
            soldierHealth = this.GetComponent<RobotHealth>();
            agent = this.GetComponent<NavMeshAgent>();
        }
        private void Start()
        {
            ChangeState(RobotState.R_Idle);
        }
        private void OnEnable()
        {
            soldierHealth.OnDie += OnDie;
        }
        private void OnDisable()
        {
            soldierHealth.OnDie -= OnDie;
        }
        private void Update()
        {
            //���� üũ
            if (soldierHealth.IsDeath)
                return;

            //�̵�
            target = new Vector3(thePlayer.position.x, 0f, thePlayer.position.z);
            float distance = Vector3.Distance(transform.position, target);

            if (distance <= attackRange)
            {
                //���� ���·� ����
                ChangeState(RobotState.R_Attack);
            }
            //��Ÿ� üũ
            if (distance <= detectingRange)
            {
                //�߰� ���·� ����
                ChangeState(RobotState.R_Chase);
            }

            //���� ����
            switch (soldierState)
            {
                case RobotState.R_Idle:
                    if(wayPoints.Length > 1)
                    {
                        idleCountdown += Time.deltaTime;
                        if(idleCountdown >= idleTime)
                        {
                            //���� ���� ����Ʈ�� �̵�
                            ChangeState(RobotState.R_Patrol);

                            //Ÿ�̸� �ʱ�ȭ
                            idleCountdown = 0f;
                        }
                    }
                    break;
                case RobotState.R_Walk:
                    break;
                case RobotState.R_Attack:
                    //���� Ÿ�̸�
                    attackCountdown += Time.deltaTime;
                    if (attackCountdown >= attackTimer)
                    {
                        //�߻�
                        animator.SetTrigger("Fire");

                        attackCountdown = 0f;
                    }
                    break;
                case RobotState.R_Death:
                    break;
                case RobotState.R_Patrol:
                    //��������Ʈ ���� ����
                    if(agent.remainingDistance <= 0.2f)
                    {
                        ChangeState(RobotState.R_Idle);
                    }
                    break;
                case RobotState.R_Chase:
                    //Ÿ���� ���� �̵�
                    agent.SetDestination(target);
                    return;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectingRange);
        }
        #endregion

        #region Custom Method
        public void ChangeState(RobotState newState)
        {
            //���� ���� üũ
            if (soldierState == newState)
            {
                return;
            }

            //���� ���¸� ���� ���·� ����
            beforeState = soldierState;
            //���ο� ���¸� ���� ���·� ����
            soldierState = newState;

            //���� ���濡 ���� ���� ����
            if(soldierState == RobotState.R_Patrol)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Walk);
                //���� ��������Ʈ�� �̵�
                GoNextWayPoint();
            }
            else if (soldierState == RobotState.R_Idle)
            {
                animator.SetInteger(enemyState, (int)soldierState);
                idleTime = Random.Range(2f, 5f);
            }
            else if (soldierState == RobotState.R_Chase)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Walk);
                //Ÿ���� ���� �̵�
                agent.SetDestination(target);
                //���� �ִ� ����
                animator.SetLayerWeight(1,1);
            }
            else if (soldierState == RobotState.R_Attack)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Idle);
                animator.SetLayerWeight(1, 1);

                //Ÿ���� ���� �̵� ����
                agent.ResetPath();
            }
            else
            {
                animator.SetInteger(enemyState, (int)soldierState);
            }                
        }

        //���� ��������Ʈ�� �̵�
        private void GoNextWayPoint()
        {
            nowPointIndex++;
            if(nowPointIndex >= wayPoints.Length)
            {
                nowPointIndex = 0;
            }
            agent.SetDestination(wayPoints[nowPointIndex].position);
        }

        private void OnDie()
        {
            ChangeState(RobotState.R_Death);

            //���� �׾����� bgm�ٽ� ���
            //bgm.Play();

            this.GetComponent<BoxCollider>().enabled = false;
        }
        #endregion
    }

}
