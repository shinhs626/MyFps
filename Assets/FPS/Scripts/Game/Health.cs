using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    //체력을 관리하는 클래스
    public class Health : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float maxHealth = 100f;

        private bool isDeath = false;

        //체력 위험 경계 비율
        [SerializeField]
        private float criticalHealthRatio = 0.2f;

        public UnityAction<float> OnHeal;       //힐하면 등록된 함수를 호출한다
        public UnityAction<float, GameObject> OnDamaged;    //데미지를 입으면 등록된 함수를 호출한다
        public UnityAction OnDie;               //죽을때 등록된 함수를 호출한다
        #endregion

        #region Property
        public float CurrentHealth { get; private set; }

        //무적 체크
        public bool Invincible { get; set; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            CurrentHealth = maxHealth;
            Invincible = false;
        }
        #endregion

        #region Custom Method
        //힐 아이템을 먹을 수 있는지 체크
        public bool CanPickUp() => CurrentHealth < maxHealth;

        //UI HP 바 게이지 량
        public float GetRatio() => CurrentHealth / maxHealth;

        //위험 체크
        public bool IsCritical() => GetRatio() <= criticalHealthRatio;

        //힐 계산
        public void Heal(float healAmount)
        {
            float beforeHealth = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            float realHeal = CurrentHealth - beforeHealth;
            if(realHeal > 0f)
            {
                //힐 구현
                //Debug.Log("Heal");
                OnHeal?.Invoke(realHeal);
            }
            
        }

        //매개변수 데미지랑, 데미지를 준 오브젝트
        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincible)
                return;

            float beforeHealth = CurrentHealth; //데미지 입기 전 체력
            //데미지 계산
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);  //체력의 최대 최소값 클램프

            //리얼 데미지
            float realDamage = beforeHealth - CurrentHealth;
            if(realDamage > 0f)
            {
                //데미지 효과 구현 - 등록된 함수를 호출한다
                OnDamaged?.Invoke(realDamage,damageSource);
            }

            //죽음 처리
            HandleDeath();
        }
        private void HandleDeath()
        {
            if (isDeath)
                return;
            if(CurrentHealth <= 0)
            {
                isDeath = true;

                //죽음 구현
                OnDie?.Invoke();
            }
        }
        #endregion
    }

}
