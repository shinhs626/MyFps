using UnityEngine;

namespace MyFps
{
    public class FHiddenSceneTrigger : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;

        [SerializeField]
        private string loadToScene = "MainMenu";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                AudioManager.Instance.Stop("Bgm");
                fader.FadeTo(loadToScene);

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
        #endregion
    }

}
