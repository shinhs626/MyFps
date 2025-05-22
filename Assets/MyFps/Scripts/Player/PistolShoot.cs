using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    //피스톨 제어 클래스
    public class PistolShoot : MonoBehaviour
    {
        #region Variables
        private Animator animator;

        public GameObject fireFlash;
        public AudioSource fireSound;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            animator = this.GetComponent<Animator>();
        }
        #endregion

        #region Custom Method
        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.started)    //keydown, buttondown
            {
                StartCoroutine(Fire());
            }
        }

        IEnumerator Fire()
        {
            animator.SetTrigger("Fire");
            fireFlash.SetActive(true);
            fireSound.Play();
            yield return new WaitForSeconds(0.5f);
            fireFlash.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        #endregion
    }

}
