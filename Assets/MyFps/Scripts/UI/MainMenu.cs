using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace MyFps
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioManager audioManager;

        public SceneFader fader;

        public GameObject OptionUI;
        public GameObject creditUI;
        public GameObject mainUI;

        //볼륨 조절
        public AudioMixer audioMixer;

        public Slider bgmSlider;
        public Slider sfxSlider;

        private bool isShowOption;
        private bool isShowCredit;

        [SerializeField]
        private string mainScene = "MainScene01";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            LoadOptions();

            //참조
            audioManager = AudioManager.Instance;

            fader.FadeStart();

            //메뉴 배경음 플레이
            audioManager.PlayBgm("MenuMusic");
        }
        private void Update()
        {
            //esc키를 누르면
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideOptionUI();
            }
            else
            {
                HideCreditUI();
            }
        }
        #endregion

        #region Custom Method
        public void MainSceneLoad()
        {
            //메뉴선택 사운드
            audioManager.Play("MenuSelect");

            fader.FadeTo(mainScene);
        }
        public void LoadScene()
        {
            Debug.Log("로드");
        }
        public void Options()
        {
            //Debug.Log("옵션");            
            OptionUI.SetActive(true);
            isShowOption = true;
        }
        public void Credits()
        {
            Debug.Log("크레딧");
            audioManager.Play("MenuSelect");

            StartCoroutine(ShowCredit());
        }
        public void Quit()
        {
            //Todo : Cheating
            PlayerPrefs.SetFloat("Bgm", 0f);
            PlayerPrefs.SetFloat("Sfx", 0f);

            Debug.Log("나가기");
            Application.Quit();
        }

        //Bgm 볼륨 조절
        public void SetBgmVolume(float value)
        {
            //볼륨 저장
            PlayerPrefs.SetFloat("Bgm", value);

            audioMixer.SetFloat("Bgm", value);
        }
        //Sfx 볼륨 조절
        public void SetSfxVolume(float value)
        {
            PlayerPrefs.SetFloat("Sfx", value);

            audioMixer.SetFloat("Sfx", value);
        }

        //옵션 저장값 가져오기
        public void LoadOptions()
        {
            //배경음 볼륨값
            float bgmVolume = PlayerPrefs.GetFloat("Bgm", 0f);
            SetBgmVolume(bgmVolume);
            //UI에 적용
            bgmSlider.value = bgmVolume;

            //Sfx 볼륨값
            float sfxVolume = PlayerPrefs.GetFloat("Sfx", 0f);
            SetSfxVolume(sfxVolume);
            //UI에 적용
            sfxSlider.value = sfxVolume;
        }
        public void HideOptionUI()
        {
            isShowOption = false;
            OptionUI.SetActive(false);
            mainUI.SetActive(true);
        }
        IEnumerator ShowCredit()
        {
            isShowCredit = true;
            creditUI.SetActive(true);
            mainUI.SetActive(false);

            yield return new WaitForSeconds(6f);

            HideCreditUI();
        }

        public void HideCreditUI()
        {
            isShowCredit = false;
            creditUI.SetActive(false);
            mainUI.SetActive(true);
        }
        #endregion
    }

}
