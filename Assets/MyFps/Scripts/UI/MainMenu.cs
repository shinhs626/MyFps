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
        //����
        private AudioManager audioManager;

        public SceneFader fader;

        public GameObject OptionUI;
        public GameObject creditUI;
        public GameObject mainUI;

        //���� ����
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

            //����
            audioManager = AudioManager.Instance;

            fader.FadeStart();

            //�޴� ����� �÷���
            audioManager.PlayBgm("MenuMusic");
        }
        private void Update()
        {
            //escŰ�� ������
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
            //�޴����� ����
            audioManager.Play("MenuSelect");

            fader.FadeTo(mainScene);
        }
        public void LoadScene()
        {
            Debug.Log("�ε�");
        }
        public void Options()
        {
            //Debug.Log("�ɼ�");            
            OptionUI.SetActive(true);
            isShowOption = true;
        }
        public void Credits()
        {
            Debug.Log("ũ����");
            audioManager.Play("MenuSelect");

            StartCoroutine(ShowCredit());
        }
        public void Quit()
        {
            //Todo : Cheating
            PlayerPrefs.SetFloat("Bgm", 0f);
            PlayerPrefs.SetFloat("Sfx", 0f);

            Debug.Log("������");
            Application.Quit();
        }

        //Bgm ���� ����
        public void SetBgmVolume(float value)
        {
            //���� ����
            PlayerPrefs.SetFloat("Bgm", value);

            audioMixer.SetFloat("Bgm", value);
        }
        //Sfx ���� ����
        public void SetSfxVolume(float value)
        {
            PlayerPrefs.SetFloat("Sfx", value);

            audioMixer.SetFloat("Sfx", value);
        }

        //�ɼ� ���尪 ��������
        public void LoadOptions()
        {
            //����� ������
            float bgmVolume = PlayerPrefs.GetFloat("Bgm", 0f);
            SetBgmVolume(bgmVolume);
            //UI�� ����
            bgmSlider.value = bgmVolume;

            //Sfx ������
            float sfxVolume = PlayerPrefs.GetFloat("Sfx", 0f);
            SetSfxVolume(sfxVolume);
            //UI�� ����
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
