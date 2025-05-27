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
            Debug.Log("�ε�");
        }
        public void Options()
        {
            Debug.Log("�ɼ�");
        }
        public void Credits()
        {
            Debug.Log("ũ����");
        }
        public void Quit()
        {
            Debug.Log("������");
        }

        #endregion
    }

}
