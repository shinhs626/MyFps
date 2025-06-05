using UnityEngine;

namespace MyFps
{
    public class SafeZoneOutTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject safeZoneInTrigger;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerController.safeZoneIn = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                safeZoneInTrigger.SetActive(true);

                this.gameObject.SetActive(false);
            }
        }
        #endregion
    }

}
