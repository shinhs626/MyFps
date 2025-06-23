using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Unity.FPS.Game
{
    //ũ�ν���� ������ ����ü
    [Serializable]
    public struct CrosshairData
    {
        public Sprite crosshairSprite;
        public float crosshairSize;
        public Color crosshairColor;
    }

    //���� ���� Ÿ�� enum
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
        Sniper
    }

    //���⸦ �����ϴ� Ŭ����, ��� ���⿡ �����ȴ�
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //���� ����� Ȱ��ȭ, ��Ȱ��
        public GameObject weaponRoot;
        public Transform weaponMuzzle;

        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;       //���� �ٲܽ� ȿ����

        //ũ�ν� ���
        public CrosshairData defaultCrosshair;      //�⺻ ũ�ν����
        public CrosshairData targetInSightCrosshair;        //���� Ÿ���� �Ǿ������� ũ�ν����

        //���� Aim
        [Range(0,1)]
        public float aimZoomRatio = 1f; //���ؽ� �� Ȯ�� ����
        public Vector3 aimOffset;       //���� ��ġ�� �̵��� ���⺰ offset(����) ��ġ

        //����
        [SerializeField]
        private WeaponShootType shootType;

        [SerializeField]
        private float maxAmmo = 8f;
        private float currentAmmo;

        [SerializeField]
        private float delayBetweenShots = 0.5f;     //�߻� �ð� ����
        private float lastTimeShot;     //������ �߻� �ð�

        //�߻� ȿ��
        public GameObject muzzleFlashPrefab;    //vfx
        public AudioClip shootSfx;              //sfx

        //�ݵ� Recoil
        public float recoilForce = 0.5f;

        //�߻�ü Projectile
        public ProjectileBase projectPrefab;

        //�ѹ� ��Ƽ踦 ��涧(��µ�) �ʿ��� �ҷ��� ����
        [SerializeField]
        private int bulletsPerShot = 1;

        //�߻�ü�� �߻�� �� ���� ������ ����
        [SerializeField]
        private float bulletSpreadAngle = 0f;

        //�߻�ü Projectile
        private Vector3 lastMuzzlePosition;     //���� �����ӿ� Muzzle�� ��ġ
        #endregion

        #region Property
        public GameObject Owner { get; set; }       //���⸦ ������ ���� ������Ʈ

        public GameObject SourcePrefab { get; set; }    //���⸦ ������ ���� ������

        public bool IsWeapon { get; set; }      //���� �� ���Ⱑ Ȱ��ȭ ���ִ��� ����

        //Projectile
        public Vector3 MuzzleWorldVelocity { get; private set; }    //�߻� �� �ѱ��� �̵� �ӵ�
        public float CurrentCharge { get; private set; }    //

        public WeaponShootType ShootType { get; private set; }    //shootType
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //����
            shootAudioSource = this.GetComponent<AudioSource>();
        }
        private void Start()
        {
            //�ʱ�ȭ
            currentAmmo = maxAmmo;
            lastTimeShot = Time.time;
        }
        private void Update()
        {
            if(Time.deltaTime > 0)
            {
                //�̹� �������� Muzzle �ӵ�
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;
            }
            
            //Muzzle ��ġ ����
            lastMuzzlePosition = weaponMuzzle.position;
        }
        #endregion

        #region Custom Method
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            IsWeapon = show;

            //���� ��ü
            if (show == true && switchWeaponSfx)
            {
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }
        }

        //�� ��ǲ ó�� : �Ű������� fire down, held, released �޾Ƽ� ���� Ÿ�� ó��
        public bool HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
        {
            switch (shootType)
            {
                case WeaponShootType.Manual:
                    if(inputDown == true)
                    {
                        //��
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Automatic:
                    if(inputHeld == true)
                    {
                        //��
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Charge:

                    break;
                case WeaponShootType.Sniper:
                    if (inputDown == true)
                    {
                        //��
                        return TryShoot();
                    }
                    break;
            }
            return false;
        }

        //�߻� ó��
        private bool TryShoot()
        {
            
            if(currentAmmo >= 1f && (lastTimeShot + delayBetweenShots) < Time.time)
            {
                currentAmmo -= bulletsPerShot;
                Debug.Log("shoot!" + currentAmmo);

                HandleShoot();

                return true;
            }
            
            return false;
        }

        //�߻� ����
        private void HandleShoot()
        {
            //bullet shot final ammo
            int bulletPerShotFinal = bulletsPerShot;
            for (int i = 0; i < bulletPerShotFinal; i++)
            {
                Vector3 shotDirector = GetShotDirectionWithinSpread(weaponMuzzle);
                ProjectileBase projectileBase = Instantiate(projectPrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDirector));
                projectileBase.Shoot(this);
            }   

            //vfx - Muzzle effect
            if (muzzleFlashPrefab)
            {
                GameObject effectGo = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
                Destroy(effectGo, 0.8f);
            }

            //sfx
            if (shootSfx)
            {
                shootAudioSource.PlayOneShot(shootSfx);
            }            

            //�߻��� �ð� ����
            lastTimeShot = Time.time;
        }

        //�߻�ü�� ������ ���� ���ϱ�
        private Vector3 GetShotDirectionWithinSpread(Transform shotTransform)
        {
            float spreadAngleRatio = bulletSpreadAngle / 100f;
            return Vector3.Slerp(shotTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }
        #endregion
    }

}
