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

    //���⸦ �����ϴ� Ŭ����, ��� ���⿡ �����ȴ�
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //���� ����� Ȱ��ȭ, ��Ȱ��
        public GameObject weaponRoot;

        public AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;       //���� �ٲܽ� ȿ����

        //ũ�ν� ���
        public CrosshairData defaultCrosshair;      //�⺻ ũ�ν����
        public CrosshairData targetInSightCrosshair;        //���� Ÿ���� �Ǿ������� ũ�ν����

        //���� Aim
        [Range(0,1)]
        public float aimZoomRatio = 1f; //���ؽ� �� Ȯ�� ����
        public Vector3 aimOffset;       //���� ��ġ�� �̵��� ���⺰ offset(����) ��ġ
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
