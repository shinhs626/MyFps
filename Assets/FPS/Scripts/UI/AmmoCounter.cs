using TMPro;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class AmmoCounter : MonoBehaviour
    {
        #region Variables
        //����
        private PlayerWeaponManager playerWeaponManager;
        private WeaponController weaponController;

        private int weaponCounterIndex;     //Ammo Counter UI �ε��� ��ȣ

        //UI
        public TextMeshProUGUI weaponIndexText;
        public Image ammoFillAmount;

        public CanvasGroup canvasGroup;
        [SerializeField]
        [Range(0,1)]
        private float unSelectedOpacity = 0.5f;     //���õ��� ���� UI ����
        private Vector3 unSelectedScale = Vector3.one * 0.8f;   //���õ��� ���� UI ũ��(80X)

        [SerializeField]
        private float ammoFillSharpness = 10f;  //ammo UI ������
        [SerializeField]
        private float weaponSwitchSharpness = 10f;  //���� ����� UI ũ�� ���� �ӵ�(Lerp ���)

        //������ �� �÷� ȿ��
        public ForBackColorChange forBackColorChange;
        #endregion

        #region Property
        public int WeaponCounterIndex => weaponCounterIndex;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            float currentFillRate = weaponController.CurrentAmmoRate;

            ammoFillAmount.fillAmount = Mathf.Lerp(ammoFillAmount.fillAmount, currentFillRate, Time.deltaTime * ammoFillSharpness);

            //��Ƽ�� ����� �ƴ� ���� ����
            bool isActiveWeapon = weaponController == playerWeaponManager.GetActiveWeapon();
            float currentOpacity = isActiveWeapon ? 1f : unSelectedOpacity;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, currentOpacity, Time.deltaTime * weaponSwitchSharpness);
            //UI ũ�� - ���� ��ü�� ���� ����
            Vector3 currentScale = isActiveWeapon ? Vector3.one : unSelectedScale;
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale, Time.deltaTime * weaponSwitchSharpness);

            //������ Bar �÷� ȿ��
            forBackColorChange.UpdateVisual(currentFillRate);
        }
        #endregion

        #region Custom Method
        //Ammo Counter UI �ʱ�ȭ
        public void Initialize(WeaponController weapon, int weaponIndex)
        {
            weaponController = weapon;
            weaponCounterIndex = weaponIndex;

            //weaponManager ��������
            playerWeaponManager = FindFirstObjectByType<PlayerWeaponManager>();

            weaponIndexText.text = (weaponIndex+1).ToString();

            forBackColorChange.Initialized(1f, 0.1f);
        }
        #endregion
    }

}
