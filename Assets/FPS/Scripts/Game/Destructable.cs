using UnityEngine;

namespace Unity.FPS.Game
{
    //�׾����� Health�� ������ �ִ� ������Ʈ�� ų
    public class Destructable : MonoBehaviour
    {
        #region Variables
        //����
        private Health health;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            health = this.GetComponent<Health>();
        }
        private void Start()
        {
            //�ʱ�ȭ
            //Health�� UnityAction �Լ� ���
            health.OnDamaged += OnDamaged;
            health.OnDie += OnDie;
        }
        #endregion

        #region Custom Method
        //Health�� UnityAction �Լ� 
        private void OnDamaged(float damage, GameObject damageSource)
        {
            //TODO: ������ ����
        }

        //Health�� UnityAction �Լ� OnDie �� ����� �Լ�
        private void OnDie()
        {
            //���� ó��

            //������Ʈ ų
            Destroy(gameObject);
        }
        #endregion
    }
}

