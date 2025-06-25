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
        //참조
        private PlayerWeaponManager playerWeaponManager;
        private WeaponController weaponController;

        private int weaponCounterIndex;     //Ammo Counter UI 인덱스 번호

        //UI
        public TextMeshProUGUI weaponIndexText;
        public Image ammoFillAmount;

        public CanvasGroup canvasGroup;
        [SerializeField]
        [Range(0,1)]
        private float unSelectedOpacity = 0.5f;     //선택되지 않은 UI 투명값
        private Vector3 unSelectedScale = Vector3.one * 0.8f;   //선택되지 않은 UI 크기(80X)

        [SerializeField]
        private float ammoFillSharpness = 10f;  //ammo UI 게이지
        [SerializeField]
        private float weaponSwitchSharpness = 10f;  //무기 변경시 UI 크기 변경 속도(Lerp 계수)

        //게이지 바 컬러 효과
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

            //액티브 무기와 아닌 무기 구분
            bool isActiveWeapon = weaponController == playerWeaponManager.GetActiveWeapon();
            float currentOpacity = isActiveWeapon ? 1f : unSelectedOpacity;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, currentOpacity, Time.deltaTime * weaponSwitchSharpness);
            //UI 크기 - 무기 교체시 연출 구현
            Vector3 currentScale = isActiveWeapon ? Vector3.one : unSelectedScale;
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale, Time.deltaTime * weaponSwitchSharpness);

            //게이지 Bar 컬러 효과
            forBackColorChange.UpdateVisual(currentFillRate);
        }
        #endregion

        #region Custom Method
        //Ammo Counter UI 초기화
        public void Initialize(WeaponController weapon, int weaponIndex)
        {
            weaponController = weapon;
            weaponCounterIndex = weaponIndex;

            //weaponManager 가져오기
            playerWeaponManager = FindFirstObjectByType<PlayerWeaponManager>();

            weaponIndexText.text = (weaponIndex+1).ToString();

            forBackColorChange.Initialized(1f, 0.1f);
        }
        #endregion
    }

}
