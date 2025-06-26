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
        [Range(0, 1)]
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

        //������ - Reload
        //�ڵ� ������
        [SerializeField]
        private bool automaticReload = true;
        private float ammoReloadRate = 1f;      //�ʴ� ������ �Ǵ� ammo�� ��
        private float ammoReloadDelay = 2f;           //���� �� �� delay �ð� ���� ������ �ð�

        //���� - Charge
        private float ammoUseOnStartCharge = 1f;    //���� �߻� ��ư�� �������� �� �� ������ �����ϱ� ���� ��
        private float ammoUseRateWhileCharging = 1f;    //�����ϰ� �ִ� ���� ammo�� �Һ�Ǵ� ��
        private float maxChargeDuration = 2f;   //�����ϴ� �� �ð�

        public float lastChargeTriggerTimesTemp;    //�����ϴ� �ð��� �����ϴ� �ӽú���
        #endregion

        #region Property
        public GameObject Owner { get; set; }       //���⸦ ������ ���� ������Ʈ

        public GameObject SourcePrefab { get; set; }    //���⸦ ������ ���� ������

        public bool IsWeapon { get; set; }      //���� �� ���Ⱑ Ȱ��ȭ ���ִ��� ����

        //Projectile
        public Vector3 MuzzleWorldVelocity { get; private set; }    //�߻� �� �ѱ��� �̵� �ӵ�

        //����
        public bool IsCharging { get; private set; }    //
        public float CurrentCharge { get; private set; }    //�� Ÿ���� ������ �߻� ������, 0 ~ 1

        public WeaponShootType ShootType => shootType;    //shootType

        public float CurrentAmmoRate { get; set; }  //���� ������ ammo ����
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
            UpdateCharge();
            UpdateAmmo();

            if (Time.deltaTime > 0)
            {
                //�̹� �������� Muzzle �ӵ�
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;
            }
            
            //Muzzle ��ġ ����
            lastMuzzlePosition = weaponMuzzle.position;
        }
        #endregion

        #region Custom Method
        private void UpdateCharge()
        {
            if(IsCharging == false)
            {
                return;
            }

            //CurrentCharge : 0 ~ 1(0%~100%)���� ����
            if(CurrentCharge < 1)
            {
                //���� ������ ��
                float chargeLeft = 1 - CurrentCharge;

                //�̹� �����ӿ� ������ ��
                float chargeAdd = 0;
                if(maxChargeDuration <= 0f)
                {
                    chargeAdd = chargeLeft;
                }
                else
                {
                    chargeAdd = (1f / maxChargeDuration) * Time.deltaTime;
                }
                chargeAdd = Mathf.Clamp(chargeAdd, 0f, chargeLeft);

                //chargeAdd �� ���� ammo �Һ��� ���Ѵ�
                float ammoChargeRequre = chargeAdd * ammoUseRateWhileCharging;
                if(ammoChargeRequre <= currentAmmo)
                {
                    UseAmmo(ammoChargeRequre);

                    CurrentCharge += chargeAdd;
                    CurrentCharge = Mathf.Clamp01(CurrentCharge);
                }
            }
        }

        //Ammo ����
        private void UpdateAmmo()
        {
            //������
            if (automaticReload && currentAmmo < maxAmmo && (lastTimeShot + ammoReloadDelay) <= Time.time && IsCharging == false)
            {
                currentAmmo += ammoReloadRate * Time.deltaTime;
                currentAmmo = Mathf.Clamp(currentAmmo, 0f, maxAmmo);
            }

            //CurrentAmmoRate ����
            if(maxAmmo == 0 || maxAmmo == Mathf.Infinity)
            {
                CurrentAmmoRate = 1f;
            }
            else
            {
                CurrentAmmoRate = currentAmmo / maxAmmo;
            }                
        }

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
                    if (inputDown == true)
                    {
                        //��
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Automatic:
                    if (inputHeld == true)
                    {
                        //��
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Charge:
                    if (inputHeld == true)
                    {
                        TryBeginCharge();
                    }
                    else if(inputUp == true)
                    {
                        return TryRelaseCharge();
                    }
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

        //���� ó��
        private bool TryBeginCharge()
        {
            if (IsCharging == false && currentAmmo >= ammoUseOnStartCharge && (lastTimeShot + delayBetweenShots) < Time.time)
            {
                UseAmmo(ammoUseOnStartCharge);

                lastChargeTriggerTimesTemp = Time.time;
                IsCharging = true;
                return true;
            }

            return false;
        }

        //���� ó�� - �� - �߻�
        private bool TryRelaseCharge()
        {
            if(IsCharging == true)
            {
                HandleShoot();

                //�ʱ�ȭ
                CurrentCharge = 0;
                IsCharging = false;
                return true;
            }

            return false;
        }

        //Ammo ����
        private void UseAmmo(float amount)
        {
            currentAmmo -= amount;
            currentAmmo = Mathf.Clamp(currentAmmo, 0f, maxAmmo);

            lastTimeShot = Time.time;
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
