using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ChargedWeaponEffectHandler : MonoBehaviour
    {
        #region Variables
        public GameObject chargingObject;
        public GameObject spiningFrame;
        public GameObject diskOrbitParticlePrefab;

        public MinMaxVector scale;      //���� ������Ʈ�� ũ��

        [SerializeField]
        private Vector3 offset;                 //��ƼŬ ������ ���� ��ġ ������
        public Transform parentTransform;       //��ƼŬ ������ ���� �� �θ� ������Ʈ
        public MinMaxFloat orbitY;              //��ƼŬ ������ �Ӽ�
        public MinMaxVector radius;              //��ƼŬ ũ�� ����

        public MinMaxFloat spiningSpeed;        //ȸ�� ������ ȸ�� �ӵ�

        //���� 
        public AudioClip chargeSound;           //���� ȿ����
        public AudioClip loopChargeWeaponSfx;   //ȸ�� ������ ȿ����

        public float fadeLoopDuration = 0.5f;   //���� ���̵� ȿ�� �ð�
        //ȿ���� ó�� ���� üũ( true : ȿ���� ����ӵ� ȿ�� ó��, false : ���� ���̵� ȿ�� ó��)
        public bool useProceduralPitchOnLoop = false;
        [Range(1.0f, 5.0f)]
        public float maxProceduralPitchValue = 2.0f;    //���� ȿ���� ��� �ӵ� Max ��

        private AudioSource audioSource;        //���� ȿ���� �÷��� �����
        private AudioSource audioSourceLoop;    //ȸ�� ������ ȿ���� �÷��� �����

        private ParticleSystem diskOrbitParticle;   //ȸ�� ��ƼŬ
        private ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule;

        private float chargeRatio;                  //������
        private float lastChargeTriggerTimesTemp;   //���� �ð� ����
        private float endChargeTime;                //���� ������ �ð�

        //����
        private WeaponController weaponController;
        #endregion

        #region Property
        public GameObject particleInstance { get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //���� audioSource ������Ʈ �߰�
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = chargeSound;
            audioSource.playOnAwake = false;

            //ȸ�� audioSource ������Ʈ �߰�
            audioSourceLoop = gameObject.AddComponent<AudioSource>();
            audioSourceLoop.clip = loopChargeWeaponSfx;
            audioSourceLoop.playOnAwake = false;
            audioSourceLoop.loop = true;
        }
        private void Update()
        {
            if(particleInstance == null)
            {
                SpawnParticleSystem();
            }

            //��ƼŬ ������Ʈ Ȱ��ȭ ���� - ��ó�� Active�϶�
            diskOrbitParticle.gameObject.SetActive(weaponController.IsWeapon);

            //������ ��������
            chargeRatio = weaponController.CurrentCharge;

            //�߻�ü ũ��
            chargingObject.transform.localScale = scale.GetValueRatio(chargeRatio);

            //ȸ�� ������ ȸ���ӵ�
            if (spiningFrame)
            {
                spiningFrame.transform.localRotation *= Quaternion.Euler(0f, spiningSpeed.GetValueRatio(chargeRatio) * Time.deltaTime, 0f);
            }

            //��ƼŬ
            velocityOverLifetimeModule.orbitalY = orbitY.GetValueRatio(chargeRatio);
            diskOrbitParticle.transform.localScale = radius.GetValueRatio(chargeRatio);

            //���� ���̵� ȿ��
            if(chargeRatio > 0f)
            {
                if(audioSourceLoop.isPlaying == false && weaponController.lastChargeTriggerTimesTemp > lastChargeTriggerTimesTemp)
                {
                    lastChargeTriggerTimesTemp = weaponController.lastChargeTriggerTimesTemp;

                    if(useProceduralPitchOnLoop == false)
                    {
                        endChargeTime = Time.time + chargeSound.length;

                        //���� ȿ���� �÷���
                        audioSource.Play();
                    }

                    //ȸ�� ȿ���� �÷���
                    audioSourceLoop.Play();
                }
                if(useProceduralPitchOnLoop == false)
                {
                    //���̵� ȿ��
                    float volumeRatio = Mathf.Clamp01((endChargeTime - Time.time - fadeLoopDuration)/fadeLoopDuration);
                    audioSource.volume = volumeRatio;
                    audioSourceLoop.volume = 1 - volumeRatio;
                }
                else
                {
                    //��� �ӵ� ȿ��
                    audioSourceLoop.pitch = Mathf.Lerp(1.0f, maxProceduralPitchValue, chargeRatio);
                }
            }
            else
            {
                audioSource.Stop();
                audioSourceLoop.Stop();
            }
        }
        #endregion

        #region Custom Method
        //��ƼŬ ������Ʈ ����
        private void SpawnParticleSystem()
        {
            particleInstance = Instantiate(diskOrbitParticlePrefab, parentTransform != null ? parentTransform : this.transform);
            particleInstance.transform.localPosition += offset; //������ġ ����

            //����
            FindReference();
        }

        private void FindReference()
        {
            weaponController = this.GetComponent<WeaponController>();
            diskOrbitParticle = particleInstance.GetComponent<ParticleSystem>();
            velocityOverLifetimeModule = diskOrbitParticle.velocityOverLifetime;
        } 
        #endregion
    }
}

