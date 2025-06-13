using UnityEngine;

namespace Unity.FPS.Game
{
    //���⸦ �����ϴ� Ŭ����, ��� ���⿡ �����ȴ�
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //���� ����� Ȱ��ȭ, ��Ȱ��
        public GameObject weaponRoot;

        public AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;       //���� �ٲܽ� ȿ����
        #endregion

        #region Property
        public GameObject Owner { get; set; }       //���⸦ ������ ���� ������Ʈ

        public GameObject SourcePrefab { get; set; }    //���⸦ ������ ���� ������

        public bool IsWeapon { get; set; }      //���� �� ���Ⱑ Ȱ��ȭ ���ִ��� ����
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //����
            shootAudioSource = this.GetComponent<AudioSource>();
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
        #endregion
    }

}
