using UnityEngine;

namespace MyFps
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioManager audioManager;

        public SceneFader fader;

        [SerializeField]
        private string mainScene = "MainScene01";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            audioManager = AudioManager.Instance;

            fader.FadeStart();

            //메뉴 배경음 플레이
            audioManager.PlayBgm("MenuMusic");
        }
        #endregion

        #region Custom Method
        public void MainSceneLoad()
        {
            //메뉴선택 사운드
            audioManager.Play("ButtonHit");

            fader.FadeTo(mainScene);
        }
        public void LoadScene()
        {
            Debug.Log("로드");
        }
        public void Options()
        {
            Debug.Log("옵션");
        }
        public void Credits()
        {
            Debug.Log("크레딧");
        }
        public void Quit()
        {
            Debug.Log("나가기");
        }

        #endregion
    }

}
