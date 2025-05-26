using System.Collections;
using UnityEngine;

namespace MyFps
{
    //�ǽ��� ���� Ŭ����
    public class PistolShoot : MonoBehaviour
    {
        #region Variables
        //����
        private Animator animator;
        //�÷���
        public GameObject fireFlash;
        //����
        public AudioSource fireSound;
        public Transform firePoint;

        //�ѱ� �߻� ����Ʈ
        public ParticleSystem muzzleFlash;
        //�ǰ� ����Ʈ - �ҷ��� �ǰݵǴ� ������ hitImpact �ߵ�
        public GameObject hitImpactPrefab;
        //hit ��� ����
        [SerializeField]
        private float impactForce = 10f;

        //�߻��ϸ� isFire�� true�� �ٲٰ� ��ٸ���
        private bool isFire = false;

        //���ݷ�
        [SerializeField]
        private float attackDamage = 5f;

        //���� ��Ÿ�
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
                //Debug.Log("��Ʈ!");
            }
            else
            {
                Gizmos.DrawRay(firePoint.position, firePoint.forward * maxAttackDis);
                //Debug.Log("��Ʈ!!");
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
            //���� ���� �߻� ��(1�ʵ���) �߻簡 �ȵǵ��� �Ѵ�
            isFire = true;

            //���̸� ���� 200�ȿ� ���� ������ �κ����� �������� ��
            RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, maxAttackDis);
            if (isHit)
            {
                Debug.Log("��Ʈ");
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
