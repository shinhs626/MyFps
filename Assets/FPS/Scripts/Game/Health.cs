using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    //ü���� �����ϴ� Ŭ����
    public class Health : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float maxHealth = 100f;

        private bool isDeath = false;

        //ü�� ���� ��� ����
        [SerializeField]
        private float criticalHealthRatio = 0.2f;

        public UnityAction<float> OnHeal;       //���ϸ� ��ϵ� �Լ��� ȣ���Ѵ�
        public UnityAction<float, GameObject> OnDamaged;    //�������� ������ ��ϵ� �Լ��� ȣ���Ѵ�
        public UnityAction OnDie;               //������ ��ϵ� �Լ��� ȣ���Ѵ�
        #endregion

        #region Property
        public float CurrentHealth { get; private set; }

        //���� üũ
        public bool Invincible { get; set; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //�ʱ�ȭ
            CurrentHealth = maxHealth;
            Invincible = false;
        }
        #endregion

        #region Custom Method
        //�� �������� ���� �� �ִ��� üũ
        public bool CanPickUp() => CurrentHealth < maxHealth;

        //UI HP �� ������ ��
        public float GetRatio() => CurrentHealth / maxHealth;

        //���� üũ
        public bool IsCritical() => GetRatio() <= criticalHealthRatio;

        //�� ���
        public void Heal(float healAmount)
        {
            float beforeHealth = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            float realHeal = CurrentHealth - beforeHealth;
            if(realHeal > 0f)
            {
                //�� ����
                //Debug.Log("Heal");
                OnHeal?.Invoke(realHeal);
            }
            
        }

        //�Ű����� ��������, �������� �� ������Ʈ
        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincible)
                return;

            float beforeHealth = CurrentHealth; //������ �Ա� �� ü��
            //������ ���
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);  //ü���� �ִ� �ּҰ� Ŭ����

            //���� ������
            float realDamage = beforeHealth - CurrentHealth;
            if(realDamage > 0f)
            {
                //������ ȿ�� ���� - ��ϵ� �Լ��� ȣ���Ѵ�
                OnDamaged?.Invoke(realDamage,damageSource);
            }

            //���� ó��
            HandleDeath();
        }
        private void HandleDeath()
        {
            if (isDeath)
                return;
            if(CurrentHealth <= 0)
            {
                isDeath = true;

                //���� ����
                OnDie?.Invoke();
            }
        }
        #endregion
    }

}
