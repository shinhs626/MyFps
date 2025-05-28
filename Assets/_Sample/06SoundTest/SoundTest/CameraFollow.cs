using UnityEngine;
namespace MySample
{
    public class CameraFollow : MonoBehaviour
    {
        #region Variables
        public Transform thePlayer;
        public Vector3 offset;
        #endregion

        #region Unity Event Method
        private void LateUpdate()
        {
            transform.position = thePlayer.position + offset;
        }
        #endregion
    }

}
