using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    public class DOpenning : MonoBehaviour
    {
        #region Variables
        //플레이어 오브젝트
        public GameObject thePlayer;
        //페이더 객체
        public SceneFader fader;

        //시나리오 대사 처리
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequenceTextA = "Again...";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //커서 제어
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            //0.플레이 캐릭터 비 활성화
            //thePlayer.SetActive(false);
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            //1. 페이드인 연출 (1초 대기후 페인드인 효과)
            fader.FadeStart();

            //2.화면 하단에 시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequenceTextA;

            yield return new WaitForSeconds(1f);

            sequenceText.text = "";

            AudioManager.Instance.PlayBgm("Bgm");

            //Todo : Cheating
            //메인 2 번 씬 설정
            PlayerDataManager.Instance.Weapon = WeaponType.Pistol;
            PlayerDataManager.Instance.AddAmmo(5);

            input.enabled = true;
        }
        #endregion
    }

}
