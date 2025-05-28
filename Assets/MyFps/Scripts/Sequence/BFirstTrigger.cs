using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

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

        public AudioSource line03;

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
            //thePlayer.SetActive(false);
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            //��� ��� :  "Looks like a weapon on that table."
            sequenceText.text = sequence;

            line03.Play();

            //1�� ������
            yield return new WaitForSeconds(1f);

            //ȭ��ǥ Ȱ��ȭ
            theArrow.SetActive(true);

            //2�� ������
            yield return new WaitForSeconds(2f);

            sequenceText.text = "";
            //�÷��� ĳ���� Ȱ��ȭ
            //thePlayer.SetActive(true);
            input.enabled = true;

        }
        #endregion
    }
}
