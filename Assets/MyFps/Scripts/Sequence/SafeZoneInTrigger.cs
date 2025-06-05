using UnityEngine;

namespace MyFps
{
    public class SafeZoneInTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject safeZoneOutTrigger;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerController.safeZoneIn = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                safeZoneOutTrigger.SetActive(true);

                this.gameObject.SetActive(false);
            }
        }
        #endregion
    }

}
