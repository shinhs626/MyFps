using UnityEngine;

namespace MySample
{
    public class CollisionTest : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"OnCollisionEnter:{collision.transform.tag}");
            MoveObject player = collision.transform.GetComponent<MoveObject>();

            if (player)
            {
                player.MoveLeft();
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            Debug.Log($"OnCollisionStay:{collision.transform.tag}");
        }
        private void OnCollisionExit(Collision collision)
        {
            Debug.Log($"OnCollisionExit:{collision.transform.tag}");
            MoveObject player = collision.transform.GetComponent<MoveObject>();

            if (player)
            {
                player.MoveLeft();

            }
        }
        #endregion
    }

}
