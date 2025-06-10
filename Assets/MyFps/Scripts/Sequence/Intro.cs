using NUnit.Framework.Constraints;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyFps
{
    public class Intro : MonoBehaviour
    {
        #region Variables
        //참조
        public SceneFader fader;
        public CinemachineSplineCart cart; //Cart

        [SerializeField]
        private string loadToScene = "MainScene01";
        private bool[] isArrive;        //이동 포인트 지점에 도착했는지 여부 체크

        [SerializeField]
        private int wayPointIndex;      //다음 이동 목표 지정

        //연출
        public Animator animator;
        public GameObject introUI;
        public GameObject theShedLight;

        private SplineAutoDolly.FixedSpeed dolly;
        [SerializeField]
        private string aroundTrigger = "Around";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            isArrive = new bool[5];
            wayPointIndex = 0;
            dolly = cart.AutomaticDolly.Method as SplineAutoDolly.FixedSpeed;
            dolly.Speed = 0f;

            //시퀀스
            StartCoroutine(PlayStartSequence());
        }
        private void Update()
        {
            //도착 판정
            if (cart.SplinePosition >= wayPointIndex && isArrive[wayPointIndex] == false)
            {
                Arrive();                
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                GoToPlayScene();
            }
        }
        #endregion

        #region Custom Method
        //목표 지점 도착
        private void Arrive()
        {
            if(wayPointIndex == isArrive.Length - 1)
            {
                StartCoroutine(PlayFinallySequence());
            }
            else
            {
                StartCoroutine(PlayMiddleSequence());
            }   
        }

        //플레이 씬 가기
        private void GoToPlayScene()
        {
            //코루틴 종료
            StopAllCoroutines();

            //배경음 종료
            AudioManager.Instance.StopBgm();

            //다음씬 가기
            fader.FadeTo(loadToScene);
        }

        //시작 시퀀스
        IEnumerator PlayStartSequence()
        {
            isArrive[0] = true;

            fader.FadeStart();

            //배경음
            AudioManager.Instance.PlayBgm("IntroBgm");

            yield return new WaitForSeconds(1f);

            animator.SetTrigger(aroundTrigger);
            yield return new WaitForSeconds(2f);

            //이동시작
            wayPointIndex = 1;
            dolly.Speed = 0.5f;
        }

        //이동시 포인트 지점 도착 시퀀스
        IEnumerator PlayMiddleSequence()
        {
            isArrive[wayPointIndex] = true;
            dolly.Speed = 0f;

            animator.SetTrigger(aroundTrigger);
            yield return new WaitForSeconds(2f);

            switch (wayPointIndex)
            {
                case 1:
                    introUI.SetActive(true);
                    dolly.Speed = 0.05f;
                    break;
                case 2:
                    introUI.SetActive(false);
                    dolly.Speed = 0.05f;
                    break;
                case 3:
                    theShedLight.SetActive(true);
                    yield return new WaitForSeconds(1f);

                    dolly.Speed = 0.15f;
                    break;

            }

            //이동시작
            wayPointIndex++;
        }

        //최종 지점 도착 시퀀스
        IEnumerator PlayFinallySequence()
        {
            isArrive[wayPointIndex] = true;

            //이동 멈춤
            dolly.Speed = 0f;

            //모든 라이트 끄기
            theShedLight.SetActive(false);

            yield return new WaitForSeconds(2f);

            //배경음 종료
            AudioManager.Instance.StopBgm();

            //다음씬 가기
            fader.FadeTo(loadToScene);
        }
        #endregion
    }

}
