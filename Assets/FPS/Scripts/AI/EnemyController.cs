using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.AI
{
    //������ �����͸� �����ϴ� ����ü
    public struct RendererIndexData
    {
        public Renderer renderer;       //
        public int materialIndex;

        //������ - �Ű������� �Է¹��� �����ͷ� �ʱ�ȭ
        public RendererIndexData(Renderer _renderer, int index)
        {
            renderer = _renderer;
            materialIndex = index;
        }
    }

    [RequireComponent(typeof(Health))]
    public class EnemyController : MonoBehaviour
    {
        #region Varaibles
        //����
        private Health health;

        public GameObject deathVfx;
        public Transform deathVfxSpawnPosition;

        //������ ó��
        public UnityAction onDamaged;       //������ ���� �� ȣ��Ǵ� �̺�Ʈ �Լ�

        public Material bodyMaterial;       //������ ȿ���� ������ ���׸���
        [GradientUsage(true)]
        public Gradient onHitBodyGradient;  //������ ȿ���� �׶���Ʈ �� ��ȯ ȿ��

        private float flashOnHitDuration = 0.5f;    //�� ��ȯ �÷��� ȿ�� �ð�

        public AudioClip damageSfx;                 //������ ���� ȿ��

        //bodyMaterial �� ���� ������ ����Ʈ
        private List<RendererIndexData> bodyRenderers = new List<RendererIndexData>();
        //MaterialPropertyBlock �Ӽ� ����
        private MaterialPropertyBlock bodyFlashMaterialPropertyBlock;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            health = this.GetComponent<Health>();
        }

        private void Start()
        {
            //health �̺�Ʈ �Լ� ���
            health.OnDamaged += OnDamaged;
            health.OnDie += OnDie;
        }
        #endregion

        #region Custom Method
        //health OnDamaged ����� ȣ��Ǵ� �Լ�
        private void OnDamaged(float damage, GameObject damageSource)
        {

        }
        //health OnDie ����� ȣ��Ǵ� �Լ�
        private void OnDie()
        {
            //�׾����� ȿ�� - Vfx
            GameObject effectGo = Instantiate(deathVfx, deathVfxSpawnPosition.position, Quaternion.identity);
            Destroy(effectGo, 5f);

            //
        }
        #endregion
    }

}
