using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    public class EJumpTrigger : MonoBehaviour
    {
        #region Variables
        public PlayerInput playerInput;

        public GameObject activeSphere;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(ActiveSphere());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator ActiveSphere()
        {
            playerInput.enabled = false;
            activeSphere.SetActive(true);
            //Debug.Log("트리거 발동");

            yield return new WaitForSeconds(1f);

            playerInput.enabled = true;
            this.GetComponent<BoxCollider>().enabled = false;
            Destroy(activeSphere);
        }
        #endregion
    }

}
