using UnityEngine;

namespace MyFps
{
    public class DoorCellExit : MonoBehaviour
    {
        #region Variables
        public Animator animator;
        public SceneFader fader;

        public AudioSource bgm;
        public AudioSource creakyDoor;

        [SerializeField]
        private string loadToScene = "MainScene02";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("¥Ÿ¿Ω æ¿");
            animator.SetBool("IsOpen", true);
            bgm.Stop();
            creakyDoor.Play();

            fader.FadeTo(loadToScene);
        }
        #endregion
    }

}
