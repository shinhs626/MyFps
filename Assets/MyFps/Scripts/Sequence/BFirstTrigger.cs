using UnityEngine;
using System.Collections;
using TMPro;

namespace MyFps
{
    //ù��° Ʈ���� ����
    public class BFirstTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject thePlayer;

        //ȭ��ǥ
        public GameObject theArrow;
        
        //�ó����� ��� ó��
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequence = "Looks like a weapon on that table";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            //�÷��̾� üũ
            if (other.tag == "Player")
            {
                //Ʈ���� ����
                this.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(SequencePlayer());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlayer()
        {
            //�÷��� ĳ���� ��Ȱ��ȭ  (�÷��� ����)
            thePlayer.SetActive(false);

            //��� ��� :  "Looks like a weapon on that table."
            sequenceText.text = sequence;

            //1�� ������
            yield return new WaitForSeconds(1f);

            //ȭ��ǥ Ȱ��ȭ
            theArrow.SetActive(true);

            //2�� ������
            yield return new WaitForSeconds(2f);

            sequenceText.text = "";
            //�÷��� ĳ���� Ȱ��ȭ
            thePlayer.SetActive(true);

        }
        #endregion
    }
}
