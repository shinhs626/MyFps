using UnityEngine;
namespace Unity.FPS.Game
{
    //�������� �Դ� �浹ü ���� �������� �������� ����ϴ� Ŭ����
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //����
        private Health health;

        //������ ���
        [SerializeField]
        private float damageMultiplier = 1.0f;

        //���� ������ ���
        [SerializeField]
        private float sensibilityToSelfDamage = 0.5f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //����
            health = this.GetComponent<Health>();
            if(health == null)
            {
                health = this.GetComponentInParent<Health>();
            }
        }
        #endregion

        #region Custom Method
        //isExplosionDamage : ���� ���ݿ� ���� ������ ���� üũ
        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {
            if (health == null)
                return;

            var totalDamage = damage;

            //���� ������ �ƴ� ��츸 ������ ��� ����
            if(isExplosionDamage == false)
            {
                //������ ��� ����
                totalDamage *= damageMultiplier;

            }
            
            //���� ������ üũ
            if(health.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfDamage;
            }

            //������ ��� �� ������ ����
            health.TakeDamage(totalDamage, damageSource);
        }
        #endregion
    }

}
