using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.AI
{
    //랜더러 데이터를 관리하는 구조체
    public struct RendererIndexData
    {
        public Renderer renderer;       //
        public int materialIndex;

        //생성자 - 매개변수로 입력받은 데이터로 초기화
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
        //참조
        private Health health;

        public GameObject deathVfx;
        public Transform deathVfxSpawnPosition;

        //데미지 처리
        public UnityAction onDamaged;       //데미지 입을 때 호출되는 이벤트 함수

        public Material bodyMaterial;       //데미지 효과를 구현할 메테리얼
        [GradientUsage(true)]
        public Gradient onHitBodyGradient;  //데미지 효과를 그라디언트 색 변환 효과

        private float flashOnHitDuration = 0.5f;    //색 변환 플래시 효과 시간

        public AudioClip damageSfx;                 //데미지 사운드 효과

        //bodyMaterial 을 가진 랜더러 리스트
        private List<RendererIndexData> bodyRenderers = new List<RendererIndexData>();
        //MaterialPropertyBlock 속성 변경
        private MaterialPropertyBlock bodyFlashMaterialPropertyBlock;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            health = this.GetComponent<Health>();
        }

        private void Start()
        {
            //health 이벤트 함수 등록
            health.OnDamaged += OnDamaged;
            health.OnDie += OnDie;
        }
        #endregion

        #region Custom Method
        //health OnDamaged 실행시 호출되는 함수
        private void OnDamaged(float damage, GameObject damageSource)
        {

        }
        //health OnDie 실행시 호출되는 함수
        private void OnDie()
        {
            //죽었을때 효과 - Vfx
            GameObject effectGo = Instantiate(deathVfx, deathVfxSpawnPosition.position, Quaternion.identity);
            Destroy(effectGo, 5f);

            //
        }
        #endregion
    }

}
