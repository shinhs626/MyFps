using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    public class Soldier : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        public Transform thePlayer; //타겟
        private NavMeshAgent agent;

        private RobotHealth soldierHealth;

        private Vector3 target;

        //로봇의 현재 상태
        private RobotState soldierState;
        //로봇의 이전 상태
        private RobotState beforeState;

        //애니메이션 파라미터
        private string enemyState = "EnemyState";

        //순찰
        public Transform[] wayPoints;
        private int nowPointIndex = 0;

        //대기 타이머
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
            //죽음 체크
            if (soldierHealth.IsDeath)
                return;

            //이동
            target = new Vector3(thePlayer.position.x, 0f, thePlayer.position.z);
            float distance = Vector3.Distance(transform.position, target);

            if (distance <= attackRange)
            {
                //공격 상태로 변경
                ChangeState(RobotState.R_Attack);
            }
            //사거리 체크
            if (distance <= detectingRange)
            {
                //추격 상태로 변경
                ChangeState(RobotState.R_Chase);
            }

            //상태 구현
            switch (soldierState)
            {
                case RobotState.R_Idle:
                    if(wayPoints.Length > 1)
                    {
                        idleCountdown += Time.deltaTime;
                        if(idleCountdown >= idleTime)
                        {
                            //다음 웨이 포인트로 이동
                            ChangeState(RobotState.R_Patrol);

                            //타이머 초기화
                            idleCountdown = 0f;
                        }
                    }
                    break;
                case RobotState.R_Walk:
                    break;
                case RobotState.R_Attack:
                    //공격 타이머
                    attackCountdown += Time.deltaTime;
                    if (attackCountdown >= attackTimer)
                    {
                        //발사
                        animator.SetTrigger("Fire");

                        attackCountdown = 0f;
                    }
                    break;
                case RobotState.R_Death:
                    break;
                case RobotState.R_Patrol:
                    //웨이포인트 도착 판정
                    if(agent.remainingDistance <= 0.2f)
                    {
                        ChangeState(RobotState.R_Idle);
                    }
                    break;
                case RobotState.R_Chase:
                    //타겟을 향해 이동
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
            //현재 상태 체크
            if (soldierState == newState)
            {
                return;
            }

            //현재 상태를 이전 상태로 저장
            beforeState = soldierState;
            //새로운 상태를 현재 상태로 저장
            soldierState = newState;

            //상태 변경에 따른 구현 내용
            if(soldierState == RobotState.R_Patrol)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Walk);
                //다음 웨이포인트로 이동
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
                //타겟을 향해 이동
                agent.SetDestination(target);
                //조준 애니 적용
                animator.SetLayerWeight(1,1);
            }
            else if (soldierState == RobotState.R_Attack)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Idle);
                animator.SetLayerWeight(1, 1);

                //타겟을 향한 이동 멈춤
                agent.ResetPath();
            }
            else
            {
                animator.SetInteger(enemyState, (int)soldierState);
            }                
        }

        //다음 웨이포인트로 이동
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

            //적인 죽었을때 bgm다시 재생
            //bgm.Play();

            this.GetComponent<BoxCollider>().enabled = false;
        }
        #endregion
    }

}
