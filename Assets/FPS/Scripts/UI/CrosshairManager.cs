using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class CrosshairManager : MonoBehaviour
    {
        #region Variables
        public Image crosshairImage;
        public Sprite nullCrosshairSprite;      //��Ƽ���� ���Ⱑ ������ �������� ũ�ν����


        //����
        private PlayerWeaponManager weaponManager;

        private RectTransform crosshairRectTransform;       //ũ�ν���� �̹��� Ʈ������(������ ����)
        private CrosshairData crosshairDefault;     //��Ƽ���� ������ �⺻ ũ�ν���� ������
        private CrosshairData crosshairTarget;      //��Ƽ���� ������ Ÿ���� ũ�ν���� ������

        private CrosshairData currentCrosshair;     //���� �������� ũ�ν����

        //���� - ������, �÷� ����
        private float crosshairUpdateSharpness = 5f;        //���� ���� �ӵ�

        //���� ���� ���� üũ
        private bool wasPointingAtEnemy;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            weaponManager = FindFirstObjectByType<PlayerWeaponManager>();
            OnWeaponChanged(weaponManager.GetActiveWeapon());

            //���� ��ü�� ȣ�� �Ǵ� �̺�Ʈ �Լ��� ���
            weaponManager.OnSwitchToWeapon += OnWeaponChanged;
        }

        private void Update()
        {
            UpdateCrosshairPointingAtEnemy(false);

            //���� ���� üũ
            wasPointingAtEnemy = weaponManager.IsPointingAtEnemy;
        }
        #endregion

        #region Custom Method
        //ũ�ν���� ������ ���� ũ�ν���� �׸���
        private void UpdateCrosshairPointingAtEnemy(bool force)
        {
            //ũ�ν���� �����Ͱ� ���� ���
            if (crosshairDefault.crosshairSprite == null)
                return;

            //�⺻ ���¿��� ���� �����ϴ� ����
            if((force || (wasPointingAtEnemy == false)) && weaponManager.IsPointingAtEnemy == true)
            {
                currentCrosshair = crosshairTarget;
                crosshairImage.sprite = currentCrosshair.crosshairSprite;
                //crosshairRectTransform.sizeDelta = currentCrosshair.crosshairSize * Vector2.one;
            }
            //���� �����ϰ� �ִٰ� ���� �Ҿ������ ����
            else if ((force || (wasPointingAtEnemy == true)) && weaponManager.IsPointingAtEnemy == false)
            {
                currentCrosshair = crosshairDefault;
                crosshairImage.sprite = currentCrosshair.crosshairSprite;
                //crosshairRectTransform.sizeDelta = currentCrosshair.crosshairSize * Vector2.one;
            }

            //����
            crosshairImage.color = Color.Lerp(crosshairImage.color, currentCrosshair.crosshairColor, crosshairUpdateSharpness * Time.deltaTime);
            crosshairRectTransform.sizeDelta = Mathf.Lerp(crosshairRectTransform.sizeDelta.x, currentCrosshair.crosshairSize, crosshairUpdateSharpness * Time.deltaTime) * Vector2.one;
        }
        
        //���� ��ü�� ȣ��Ǵ� �Լ� - ũ�ν���� ���� ������Ʈ
        private void OnWeaponChanged(WeaponController newWeapon)
        {
            if (newWeapon)
            {
                crosshairImage.enabled = true;
                crosshairRectTransform = crosshairImage.GetComponent<RectTransform>();

                //���� ��Ƽ���� ������ ũ�ν���� ������ ������ ����
                crosshairDefault = newWeapon.defaultCrosshair;
                crosshairTarget = newWeapon.targetInSightCrosshair;
            }
            else //���ο� ���Ⱑ ������ 
            {
                if (nullCrosshairSprite)
                {
                    crosshairImage.sprite = nullCrosshairSprite;
                }
                else
                {
                    crosshairImage.enabled = false;
                }
            }

            UpdateCrosshairPointingAtEnemy(true);
        }
        #endregion
    }

}
