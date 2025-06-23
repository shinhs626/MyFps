using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Unity.FPS.Game
{
    //크로스헤어 데이터 구조체
    [Serializable]
    public struct CrosshairData
    {
        public Sprite crosshairSprite;
        public float crosshairSize;
        public Color crosshairColor;
    }

    //무기 슛팅 타입 enum
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
        Sniper
    }

    //무기를 제어하는 클래스, 모든 무기에 부착된다
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //무기 비쥬얼 활성화, 비활성
        public GameObject weaponRoot;
        public Transform weaponMuzzle;

        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;       //무기 바꿀시 효과음

        //크로스 헤어
        public CrosshairData defaultCrosshair;      //기본 크로스헤어
        public CrosshairData targetInSightCrosshair;        //적이 타겟팅 되었을때의 크로스헤어

        //조준 Aim
        [Range(0,1)]
        public float aimZoomRatio = 1f; //조준시 줌 확대 배율
        public Vector3 aimOffset;       //조준 위치로 이동시 무기별 offset(조정) 위치

        //슛팅
        [SerializeField]
        private WeaponShootType shootType;

        [SerializeField]
        private float maxAmmo = 8f;
        private float currentAmmo;

        [SerializeField]
        private float delayBetweenShots = 0.5f;     //발사 시간 간격
        private float lastTimeShot;     //마지막 발사 시간

        //발사 효과
        public GameObject muzzleFlashPrefab;    //vfx
        public AudioClip shootSfx;              //sfx

        //반동 Recoil
        public float recoilForce = 0.5f;

        //발사체 Projectile
        public ProjectileBase projectPrefab;

        //한번 방아쇠를 당길때(쏘는데) 필요한 불렛의 갯수
        [SerializeField]
        private int bulletsPerShot = 1;

        //발사체가 발사될 때 퍼져 나가는 각도
        [SerializeField]
        private float bulletSpreadAngle = 0f;

        //발사체 Projectile
        private Vector3 lastMuzzlePosition;     //지난 프레임에 Muzzle의 위치
        #endregion

        #region Property
        public GameObject Owner { get; set; }       //무기를 장착한 주인 오브젝트

        public GameObject SourcePrefab { get; set; }    //무기를 생성한 원본 프리팹

        public bool IsWeapon { get; set; }      //현재 이 무기가 활성화 되있는지 인지

        //Projectile
        public Vector3 MuzzleWorldVelocity { get; private set; }    //발사 시 총구의 이동 속도
        public float CurrentCharge { get; private set; }    //

        public WeaponShootType ShootType { get; private set; }    //shootType
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            shootAudioSource = this.GetComponent<AudioSource>();
        }
        private void Start()
        {
            //초기화
            currentAmmo = maxAmmo;
            lastTimeShot = Time.time;
        }
        private void Update()
        {
            if(Time.deltaTime > 0)
            {
                //이번 프레임의 Muzzle 속도
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;
            }
            
            //Muzzle 위치 저장
            lastMuzzlePosition = weaponMuzzle.position;
        }
        #endregion

        #region Custom Method
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            IsWeapon = show;

            //무기 교체
            if (show == true && switchWeaponSfx)
            {
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }
        }

        //슛 인풋 처리 : 매개변수로 fire down, held, released 받아서 슛팅 타입 처리
        public bool HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
        {
            switch (shootType)
            {
                case WeaponShootType.Manual:
                    if(inputDown == true)
                    {
                        //슛
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Automatic:
                    if(inputHeld == true)
                    {
                        //슛
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Charge:

                    break;
                case WeaponShootType.Sniper:
                    if (inputDown == true)
                    {
                        //슛
                        return TryShoot();
                    }
                    break;
            }
            return false;
        }

        //발사 처리
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

        //발사 연출
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

            //발사한 시간 저장
            lastTimeShot = Time.time;
        }

        //발사체가 나가는 방향 구하기
        private Vector3 GetShotDirectionWithinSpread(Transform shotTransform)
        {
            float spreadAngleRatio = bulletSpreadAngle / 100f;
            return Vector3.Slerp(shotTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }
        #endregion
    }

}
