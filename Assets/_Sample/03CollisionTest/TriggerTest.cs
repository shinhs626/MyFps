using UnityEngine;

namespace MySample
{
    public class TriggerTest : MonoBehaviour
    {
        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            MoveObject player = other.transform.GetComponent<MoveObject>();

            if (player)
            {
                player.MoveRight();
                player.ChangeColor();

            }
        }
        private void OnTriggerStay(Collider other)
        {
            
        }
        private void OnTriggerExit(Collider other)
        {
            MoveObject player = other.transform.GetComponent<MoveObject>();

            if (player)
            {
                player.MoveRight();
                player.OriginColor();
            }
        }
        #endregion

    }

}
