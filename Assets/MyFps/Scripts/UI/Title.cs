using System.Collections;
using TMPro;
using UnityEngine;

namespace MyFps
{
    public class Title : MonoBehaviour
    {
        //Ÿ��Ʋ ���� �����ϴ� Ŭ���� : 3�� �Ŀ� any key���̱�

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
            //anykey�� �����Ŀ� �ƹ�Ű�� ������ ���θ޴� ����
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
        //�ڷ�ƾ �Լ�
        IEnumerator AnyKey()
        {
            yield return new WaitForSeconds(3f);

            anyKeyText.SetActive(true);
        }
        #endregion
    }

}
