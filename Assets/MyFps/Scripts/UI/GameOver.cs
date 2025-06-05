using UnityEngine;

namespace MyFps
{
    public class GameOver : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainScene01";
        [SerializeField]
        private string loadToMain = "MainMenu";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //FadeIn È¿°ú
            fader.FadeStart(0f);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        #endregion
        #region Custom Method
        public void RestartButton()
        {
            fader.FadeTo(loadToScene);
        }
        public void MenuButton()
        {
            fader.FadeTo(loadToMain);
        }
        #endregion
    }

}
