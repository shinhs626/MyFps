using UnityEngine;

namespace MyFps
{
    //로봇 상태
    public enum ZoziState
    {
        Z_Idle = 0,
        Z_Walk,
        Z_Attack,
        Z_Death
    }

    public class Zozi : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        public Transform thePlayer;

        //로봇의 현재 상태
        private ZoziState zoziState;
        private ZoziState beforeState;

        //체력
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        private string enemyState = "EnemyState";

        [SerializeField] private float moveSpeed = 2f;

        [SerializeField] private float attackRange = 1.5f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            animator = this.GetComponent<Animator>();
            currentHealth = maxHealth;
        }
        private void Update()
        {
            //이동
            Vector3 target = new Vector3(thePlayer.position.x, 0f, thePlayer.position.z);
            Vector3 dir = target - this.transform.position;
            float distance = Vector3.Distance(transform.position, thePlayer.position);

            //사거리 체크
            if(distance <= attackRange)
            {
                //2초마다 한 번씩 공격
                //플레이어가 공격범위를 벗어나면 다시 걷기 상태로 변경
                //플레이어에게 데미지 5를 준다
                ChangeState(ZoziState.Z_Attack);
            }

            //상태
            switch (zoziState)
            {
                case ZoziState.Z_Idle:
                    break;
                case ZoziState.Z_Walk:
                    transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);
                    transform.LookAt(target);
                    break;
                case ZoziState.Z_Attack:
                    break;
                case ZoziState.Z_Death:
                    break;
            }
        }
        private void OnEnable()
        {
            ChangeState(ZoziState.Z_Idle);
        }
        #endregion

        #region Custom Method
        //데미지 입기
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            //데미지 연출


            if(currentHealth <= 0f)
            {
                Die();
            }
        }
        //죽기
        public void Die()
        {
            isDeath = true;

            //죽음 처리
            ChangeState(ZoziState.Z_Death);

            //보상 처리
        }

        //상태를 매개변수로 받아 상태를 셋팅
        public void ChangeState(ZoziState newState)
        {
            if(zoziState == newState)
            {
                return;
            }
            //현재 상태를 이전 상태로 저장
            beforeState = zoziState;
            //새로운 상태를 현재 상태로 저장
            zoziState = newState;

             animator.SetInteger(enemyState, (int)zoziState);
        }
        #endregion
    }

}
