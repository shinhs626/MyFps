using UnityEngine;

namespace MySample
{
    public class TriggerTest : MonoBehaviour
    {
        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter : {other.tag}");
            //�÷��̾ ���������� 3��ŭ ���� �ش�, ���������� �ٲ۴�
            MoveObejct player = other.GetComponent<MoveObejct>();
            if(player)
            {
                player.MoveRight();
                player.ChangeColorRed();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log($"OnTriggerStay : {other.tag}");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"OnTriggerExit : {other.tag}");
            //�÷��̾ ���������� 3��ŭ ���� �ش�, �������� �÷��� �ٲ۴�
            MoveObejct player = other.GetComponent<MoveObejct>();
            if (player)
            {
                player.MoveRight();
                player.ChangeColorOrigin();
            }
        }
        #endregion
    }
}
