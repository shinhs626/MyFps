using UnityEngine;
using UnityEngine.Events;

namespace MyFps
{
    public class RobotHealth : MonoBehaviour, IDamageable
    {
        #region Variables
        //ü��
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private static bool isDeath = false;

        [SerializeField]
        private float destroyDelay = 5f;

        //���� �� ȣ��Ǵ� �̺�Ʈ �Լ�
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
            //�ʱ�ȭ
            currentHealth = maxHealth;
        }
        #endregion

        #region Custom Method
        //������ �Ա�
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            //������ ���� (Sfx, Vfx)

            if (currentHealth <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //�ױ�
        private void Die()
        {
            isDeath = true;

            //���� ó��
            OnDie?.Invoke();

            //ų
            Destroy(this.gameObject, destroyDelay);

            //����ó��..
        }
        #endregion
    }

}
