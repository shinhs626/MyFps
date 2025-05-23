using UnityEngine;
using UnityEngine.Events;

namespace MyFps
{
    public class RobotHealth : MonoBehaviour, IDamageable
    {
        #region Variables
        //체력
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private static bool isDeath = false;

        [SerializeField]
        private float destroyDelay = 5f;

        //죽음 시 호출되는 이벤트 함수
        public UnityAction OnDie;
        #endregion

        #region Property
        public bool IsDeath
        {
            get
            {
                return isDeath;
            }
        }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            currentHealth = maxHealth;
        }
        #endregion

        #region Custom Method
        //데미지 입기
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            //데미지 연출 (Sfx, Vfx)

            if (currentHealth <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽기
        private void Die()
        {
            isDeath = true;

            //죽음 처리
            OnDie?.Invoke();

            //킬
            Destroy(this.gameObject, destroyDelay);

            //보상처리..
        }
        #endregion
    }

}
