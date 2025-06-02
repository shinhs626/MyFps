using UnityEngine;

namespace MyFps
{
    public class FlyingObject : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == "Ground" && collision.relativeVelocity.magnitude > 1.0f)
            {
                //Debug.Log("¹Ù´Ú¿¡ ºÎµúÇû´Ù");
                AudioManager.Instance.Play("DoorBang2");
            }
        }
        #endregion

        #region Custom Method

        #endregion
    }

}
