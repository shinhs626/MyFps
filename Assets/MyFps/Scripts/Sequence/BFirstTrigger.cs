using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MyFps
{
    public class BFirstTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject player;
        public TextMeshProUGUI sequenceText;
        public GameObject theArrow;

        [SerializeField]
        private string sequence = "Looks like a weapon on that table";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                //트리거 해제
                this.GetComponent<BoxCollider>().enabled = false;

                StartCoroutine(SequencePlay());
            }     
        }

        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            player.SetActive(false);

            //2.화면 하단에시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequence.ToString();

            //3. 3초후에 시나리오 텍스트 없어진다
            yield return new WaitForSeconds(1f);
            theArrow.SetActive(true);
            yield return new WaitForSeconds(1f);

            sequenceText.text = "";

            //4.플레이어 캐릭터 활성화
            player.SetActive(true);
        }
        #endregion
    }

}
