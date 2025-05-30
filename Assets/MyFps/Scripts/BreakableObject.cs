using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace MyFps
{
    //
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        #region Variables
        public GameObject fakeObject;
        public GameObject realObject;
        public GameObject sphereObject;

        //숨겨진 아이템
        public GameObject hiddenItemPrefab;

        private float health = 1f;

        private bool isDeath = false;

        [SerializeField]
        private bool unBreakable = false;   //깨지지 않는 오브젝트

        [SerializeField]
        private Vector3 offset;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage)
        {
            //무적모드
            if (unBreakable)
                return;

            health -= damage;

            if(health <= 0f)
            {
                Die();
            }
        }
        private void Die()
        {
            isDeath = true;

            if (isDeath)
            {
                StartCoroutine(Break());
            }
            
        }

        IEnumerator Break()
        {
            realObject.SetActive(false);

            if (sphereObject)
            {
                yield return new WaitForSeconds(0.1f);
                sphereObject.SetActive(true);
            }

            AudioManager.Instance.Play("PotterySmash");
            fakeObject.SetActive(true);

            this.GetComponent<BoxCollider>().enabled = false;
            sphereObject.GetComponent<SphereCollider>().enabled = false;

            //숨겨진 아이템 나타내기
            if (hiddenItemPrefab)
            {
                Instantiate(hiddenItemPrefab, transform.position + offset, Quaternion.identity);
            }

            yield return new WaitForSeconds(3f);

            Destroy(this.gameObject);
        }

        
        
        #endregion
    }

}
