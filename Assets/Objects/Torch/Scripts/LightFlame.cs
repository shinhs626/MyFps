using System.Collections;
using UnityEngine;

namespace Sample
{
    public class LightFlame : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Animator targetAnimator;  // 다른 오브젝트 Animator 참조

        [SerializeField] private string flameParamName = "LightMod";  // Animator 파라미터 이름
        #endregion

        private void Start()
        {
            if (targetAnimator == null)
            {
                Debug.LogError($"{gameObject.name} 에 LightFlame 스크립트에서 targetAnimator가 할당되지 않았습니다.");
                this.enabled = false;
                return;
            }

            InvokeRepeating(nameof(Flame), 1f, 1f);
        }

        public void Flame()
        {
            int randomIndex = Random.Range(1, 4);   // 1~3
            targetAnimator.SetInteger(flameParamName, randomIndex);
        }
    }
}
