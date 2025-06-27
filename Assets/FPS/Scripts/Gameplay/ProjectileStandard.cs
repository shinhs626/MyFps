using UnityEngine;
using Unity.FPS.Game;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    public class ProjectileStandard : ProjectileBase
    {
        #region Variables
        //����
        private ProjectileBase projectileBase;  //�ڽ��� �θ�Ŭ���� ��ü
        [SerializeField]
        private float maxLifeTime = 5f;         //�߻�ü ������ Ÿ��

        //�̵�
        public float speed = 20f;
        //�߷� ����
        public float gravityDown = 0f;        

        public Transform root;      //�߻�ü�� ��ġ
        public Transform tip;      //�߻�ü�� �Ӹ� ��ġ

        private Vector3 lastRootPosition;       //�߻�ü�� ���� �����ӿ����� ��ġ
        private Vector3 velocity;               //�߻�ü�� �ӵ�

        private float shootTime;

        //�浹
        public float radius = 0.01f;       //�浹üũ �ݰ�

        public LayerMask hittableLayers = -1;   //�浹 ������ ���̾�
        private List<Collider> ignoredColliders;    //�浹 üũ ���� �ݶ��̴� ����Ʈ

        //�浹ó��
        public GameObject impactVfxPrefab;
        private float impactVfxLifeTime = 5f;       //����Ʈ ������ Ÿ��
        private float impactVfxSpawnOffset = 0.1f;  //

        public AudioClip impactSfxClip;

        //������
        public float damage = 40f;

        private DamageArea damageArea;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            projectileBase = GetComponent<ProjectileBase>();
            projectileBase.OnShoot += OnShoot;

            //����
            damageArea = this.GetComponent<DamageArea>();

            //���� �� ������ Ÿ�� �� ų
            Destroy(gameObject, maxLifeTime);
        }
        private void Update()
        {
            //�̵�
            transform.position += velocity * Time.deltaTime;

            //�߷�
            if(gravityDown > 0f)
            {
                velocity += Vector3.down * gravityDown * Time.deltaTime;
            }

            //�浹 üũ
            bool foundHit = false;

            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;

            //Sphere Cast All
            Vector3 displacementLastFrame = tip.position - lastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(lastRootPosition, radius, displacementLastFrame.normalized, displacementLastFrame.magnitude, hittableLayers, QueryTriggerInteraction.Collide);

            foreach (var hit in hits)
            {
                //���� ����� hit ã��
                if(IsHitValid(hit) && hit.distance < closestHit.distance)
                {
                    closestHit = hit;
                    foundHit = true;
                }
            }

            //�浹ü�� ã�Ҵٸ�
            if (foundHit)
            {
                if(closestHit.distance < 0f)
                {
                    closestHit.point = root.position;
                    closestHit.normal = -this.transform.forward;
                }

                //�浹 ó�� : ������ �ֱ�
                OnHit(closestHit.point, closestHit.normal, closestHit.collider);
            }

            //
            lastRootPosition = root.position;
        }
        #endregion

        #region Custom Method
        private new void OnShoot()
        {
            //�ʱ�ȭ
            velocity = transform.forward * speed;
            transform.position += projectileBase.InheritedMuzzleVelocity * Time.deltaTime;

            lastRootPosition = root.position;

            //��� �ڽ� �浹ü�� �����ͼ� �浹 üũ ���� ����Ʈ�� ���
            ignoredColliders = new List<Collider>();
            Collider[] ownerColliders = projectileBase.Owner.GetComponentsInChildren<Collider>();
            ignoredColliders.AddRange(ownerColliders);

            //��� ���� ��(�浹ü) üũ�Ͽ� �մ� ���� ����
            PlayerWeaponManager playerWeaponManager = projectileBase.Owner.GetComponent<PlayerWeaponManager>();
            if (playerWeaponManager)
            {
                Vector3 cameraToMuzzle = projectileBase.InitialPosition - playerWeaponManager.weaponCamera.transform.position;
                if (Physics.Raycast(playerWeaponManager.weaponCamera.transform.position, cameraToMuzzle.normalized, out RaycastHit hit, cameraToMuzzle.magnitude,hittableLayers, QueryTriggerInteraction.Collide))
                {
                    if (IsHitValid(hit))
                    {
                        OnHit(hit.point, hit.normal, hit.collider);
                    }
                }
            }
        }
        
        //hit�� �浹ü�� ��ȿ�� �浹ü�ΰ�
        private bool IsHitValid(RaycastHit hit)
        {
            //IgnoreHitDetection ������Ʈ�� ���� �浹ü�� ��ȿ
            if (hit.collider.GetComponent<IgnoreHitDetection>())
            {
                return false;
            }

            //Trriger && damageable ���� �浹ü
            if(hit.collider.isTrigger && hit.collider.GetComponent<Damageable>() == null)
            {
                return false;
            }


            //ignoredColliders ����Ʈ�� ������ ��ȿ
            if(ignoredColliders != null && ignoredColliders.Contains(hit.collider))
            {
                return false;
            }
            return true;
        }

        //�浹ó��
        private void OnHit(Vector3 point, Vector3 normal, Collider collider)
        {
            //������ �ֱ�
            if (damageArea)     //damageArea ������Ʈ�� ������ ���� ����
            {
                damageArea.InflictDamageArea(damage, point, hittableLayers, QueryTriggerInteraction.Collide, Owner);
            }
            else
            {
                //������
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable)
                {
                    damageable.InflictDamage(damage, false, projectileBase.Owner);
                }
            }

            //vfx
            if (impactVfxPrefab)
            {
                GameObject impactObject = Instantiate(impactVfxPrefab, point + (normal * impactVfxSpawnOffset),Quaternion.LookRotation(normal));
                if(impactVfxLifeTime > 0f)
                {
                    Destroy(impactObject, impactVfxLifeTime);
                }
            }
            //sfx
            if (impactSfxClip)
            {
                AudioUtility.CreateSFX(impactSfxClip, point, 1f, 3f);
            }

            //�߻�ü ų
            Destroy(this.gameObject);
        }
        #endregion
    }

}
