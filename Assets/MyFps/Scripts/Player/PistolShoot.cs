using System.Collections;
using UnityEngine;

namespace MyFps
{
    //피스톨 제어 클래스
    public class PistolShoot : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        //플래쉬
        public GameObject fireFlash;
        //사운드
        public AudioSource fireSound;
        public Transform firePoint;

        //총구 발사 이펙트
        public ParticleSystem muzzleFlash;
        //피격 이펙트 - 불렛에 피격되는 지점에 hitImpact 발동
        public GameObject hitImpactPrefab;
        //hit 충격 강도
        [SerializeField]
        private float impactForce = 10f;

        //발사하면 isFire를 true로 바꾸고 기다리기
        private bool isFire = false;

        //공격력
        [SerializeField]
        private float attackDamage = 5f;

        //공격 사거리
        [SerializeField]
        private float maxAttackDis = 200f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            animator = this.GetComponent<Animator>();
        }
        private void OnDrawGizmosSelected()
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, maxAttackDis);

            Gizmos.color = Color.red;
            if (isHit)
            {
                Gizmos.DrawRay(firePoint.position, firePoint.forward * hit.distance);
                //Debug.Log("히트!");
            }
            else
            {
                Gizmos.DrawRay(firePoint.position, firePoint.forward * maxAttackDis);
                //Debug.Log("히트!!");
            }
        }
        #endregion

        #region Custom Method
        public void Shoot()
        {
            if (isFire)
                return;

            if (PlayerDataManager.Instance.UseAmmo(1))
            {
                StartCoroutine(Fire());
            }     
        }

        IEnumerator Fire()
        {
            //연사 방지 발사 후(1초동안) 발사가 안되도록 한다
            isFire = true;

            //레이를 쏴서 200안에 적이 있으면 로봇에게 데미지를 줌
            RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, maxAttackDis);
            if (isHit)
            {
                Debug.Log("히트");
                //Robot robot = hit.transform.GetComponent<Robot>();
                //if(robot != null)
                //{
                //    robot.TakeDamage(attackDamage);
                //}
                //ZombieRobot zombieRobot = hit.transform.GetComponent<ZombieRobot>();
                //if (zombieRobot != null)
                //{
                //    zombieRobot.TakeDamage(attackDamage);
                //}
                //hit.point
                if (hitImpactPrefab)
                {
                    GameObject effectGo = Instantiate(hitImpactPrefab, hit.point, Quaternion.identity);
                    Destroy(effectGo, 2f);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
                }

                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                }
            }

            animator.SetTrigger("Fire");

            fireFlash.SetActive(true);
            if (muzzleFlash)
            {
                muzzleFlash.Play();
            }

            fireSound.Play();

            yield return new WaitForSeconds(0.3f);
            fireFlash.SetActive(false);
            if (muzzleFlash)
            {
                muzzleFlash.Stop();
            }

            yield return new WaitForSeconds(0.2f);

            isFire = false;
        }
        #endregion
    }

}
