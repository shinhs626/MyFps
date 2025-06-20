using System;
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
    }

    //���⸦ �����ϴ� Ŭ����, ��� ���⿡ �����ȴ�
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //���� ����� Ȱ��ȭ, ��Ȱ��
        public GameObject weaponRoot;
        public Transform weaponMuzzle;

        public AudioSource shootAudioSource;
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
        private Vector3 lastMuzzlePosition;     //���� �����ӿ� Muzzle�� ��ġ
        #endregion

        #region Property
        public GameObject Owner { get; set; }       //���⸦ ������ ���� ������Ʈ

        public GameObject SourcePrefab { get; set; }    //���⸦ ������ ���� ������

        public bool IsWeapon { get; set; }      //���� �� ���Ⱑ Ȱ��ȭ ���ִ��� ����

        //Projectile
        public Vector3 MuzzleWorldVelocity { get; private set; }    //�߻� �� �ѱ��� �̵� �ӵ�
        public float CurrentCharge { get; private set; }    //
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
            }
            return false;
        }

        //�߻� ó��
        private bool TryShoot()
        {
            
            if(currentAmmo >= 1f && (lastTimeShot + delayBetweenShots) < Time.time)
            {
                currentAmmo -= 1f;
                Debug.Log("shoot!" + currentAmmo);

                HandleShoot();

                return true;
            }
            
            return false;
        }

        //�߻� ����
        private void HandleShoot()
        {
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
        #endregion
    }

}
