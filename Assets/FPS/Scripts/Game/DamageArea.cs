using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    //������ ���� �ȿ� �ִ� Damageable �ݶ��̴� ������Ʈ���� �Ÿ��� ���� ������ �ֱ�
    public class DamageArea : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float areaOfEffectDistance = 5f;    //����(������) ������ �޴� �Ÿ�

        [SerializeField]
        private AnimationCurve damageRatioOverDistance; //Ŀ�� ��� ���� �������� ���
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        public void InflictDamageArea(float damage, Vector3 center, LayerMask layers, QueryTriggerInteraction interaction, GameObject owner)
        {
            //���� �ȿ� �ִ� ���� ����
            Dictionary<Health, Damageable> uniqueDamagedHealth = new Dictionary<Health, Damageable>();

            //���� ���� �ȿ� �ִ� ��� �浹ü ��������
            Collider[] effectedColliders = Physics.OverlapSphere(center, areaOfEffectDistance, layers, interaction);
            foreach(var collider in effectedColliders)
            {
                //damageable �ִ� �ݶ��̴� ã�� health�� Ű�� �Ͽ� uniqueDamagedHealth ���
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    //�ߺ� üũ
                    if (health && uniqueDamagedHealth.ContainsKey(health) == false)
                    {
                        uniqueDamagedHealth.Add(health, damageable);
                    }
                }

                //uniqueDamagedHealth�� �ִ� damageable���Ը� ������ �ֱ�
                foreach(var uniqueDamageable in uniqueDamagedHealth.Values)
                {
                    //������������ �Ÿ� ���ϱ�
                    float distance = Vector3.Distance(uniqueDamageable.transform.position, center);
                    //�Ÿ��� ���� ������ ���ϱ�
                    float curveDamage = damage * damageRatioOverDistance.Evaluate(distance / areaOfEffectDistance);
                    uniqueDamageable.InflictDamage(damage, true, owner);
                }
            }
        }
        #endregion
    }

}
