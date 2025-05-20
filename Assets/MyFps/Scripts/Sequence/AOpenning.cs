using MyDefence;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MyFps
{
    //플레이 씬 오프닝 연출
    public class AOpenning : MonoBehaviour
    {
        #region Variables
        public GameObject player;

        public SceneFader fader;

        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequence = "I need get out of here";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(SequencePlay());
        }

        IEnumerator SequencePlay()
        {
            player.SetActive(false);

            //1.페이드 인 연출
            fader.FadeStart(1f);

            //2.화면 하단에시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequence.ToString();

            //3. 3초후에 시나리오 텍스트 없어진다
            yield return new WaitForSeconds(3f);

            sequenceText.text = "";

            //4.플레이어 캐릭터 활성화
            player.SetActive(true);
        }
        #endregion
    }

}
