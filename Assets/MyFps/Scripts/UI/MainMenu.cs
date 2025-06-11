using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
        public GameObject loadGameButton;

        //볼륨 조절
        public AudioMixer audioMixer;

        public Slider bgmSlider;
        public Slider sfxSlider;

        private bool isShowOption = false;
        private bool isShowCredit = false;

        [SerializeField]
        private string intro = "Intro";
        //[SerializeField]
        //private string mainScene01 = "MainScene01";
        //[SerializeField]
        //private string mainScene02 = "MainScene02";

        private int sceneNumber;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            GameDataInit();

            if (sceneNumber >= 0)
            {
                loadGameButton.SetActive(true);
            }
            else
            {
                loadGameButton.SetActive(false);
            }
            //참조
            audioManager = AudioManager.Instance;

            fader.FadeStart();

            //메뉴 배경음 플레이
            audioManager.PlayBgm("MenuMusic");

            //초기화
            isShowCredit = false;
            isShowOption = false;
        }
        private void Update()
        {
            //esc키를 누르면
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isShowOption)
                {
                    HideOptionUI();
                }
                else if(isShowCredit)
                {
                    HideCreditUI();
                }
            }
            
        }
        #endregion

        #region Custom Method
        private void GameDataInit()
        {
            //옵션 저장값 가져와서 게임에 적용
            LoadOptions();

            if (PlayerDataManager.Instance == null)
            {
                Debug.LogError("PlayerDataManager 인스턴스가 null입니다! 씬에 추가되어 있는지 확인하세요.");
                return;
            }
                //게임 플레이 저장값 가져오기
                //sceneNumber = PlayerPrefs.GetInt("SaveScene", -1);

            PlayData playData = SaveLoad.LoadData();
            if(playData == null)
            {
                Debug.Log("여기");
            }
            PlayerDataManager.Instance.InitPlayerData(playData);
            sceneNumber = PlayerDataManager.Instance.SceneNumber;
        }

        public void MainSceneLoad()
        {
            //메뉴선택 사운드
            audioManager.Play("MenuSelect");
            audioManager.StopBgm();

            fader.FadeTo(intro);
        }
        public void LoadScene()
        {
            audioManager.Play("MenuSelect");
            audioManager.StopBgm();

            fader.FadeTo(sceneNumber);
        }
        public void Options()
        {
            audioManager.Play("MenuSelect");

            //Debug.Log("옵션");            
            OptionUI.SetActive(true);
            mainUI.SetActive(false);
            isShowOption = true;
        }
        public void Credits()
        {
            //Debug.Log("크레딧");
            audioManager.Play("MenuSelect");

            StartCoroutine(ShowCredit());
        }
        public void Quit()
        {
            audioManager.Play("MenuSelect");
            //Todo : Cheating
            PlayerPrefs.DeleteAll();

            //Debug.Log("나가기");
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
