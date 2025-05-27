using UnityEngine;

namespace MyFps
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;

        [SerializeField]
        private string mainScene = "MainScene01";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            fader.FadeStart();
        }
        #endregion

        #region Custom Method
        public void MainSceneLoad()
        {
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
