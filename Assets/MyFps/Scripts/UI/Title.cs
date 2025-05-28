using System.Collections;
using TMPro;
using UnityEngine;

namespace MyFps
{
    public class Title : MonoBehaviour
    {
        //타이틀 씬을 관리하는 클래스 : 3초 후에 any key보이기

        #region Variables
        public SceneFader fader;

        public GameObject anyKeyText;

        [SerializeField]
        private string loadToScene = "MainMenu";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            fader.FadeStart();

            AudioManager.Instance.PlayBgm("TitleBgm");

            StartCoroutine(AnyKey());
        }
        private void Update()
        {
            //anykey가 보인후에 아무키나 누르면 메인메뉴 가기
            if (anyKeyText)
            {
                if (Input.anyKeyDown)
                {
                    StopAllCoroutines();
                    AudioManager.Instance.Stop("TitleBgm");
                    fader.FadeTo(loadToScene);                    
                }
            }
        }
        #endregion

        #region Custom Method
        //코루틴 함수
        IEnumerator AnyKey()
        {
            yield return new WaitForSeconds(3f);

            anyKeyText.SetActive(true);
        }
        #endregion
    }

}
