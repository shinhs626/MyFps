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
        //����
        public SceneFader fader;
        public CinemachineSplineCart cart; //Cart

        [SerializeField]
        private string loadToScene = "MainScene01";
        private bool[] isArrive;        //�̵� ����Ʈ ������ �����ߴ��� ���� üũ

        [SerializeField]
        private int wayPointIndex;      //���� �̵� ��ǥ ����

        //����
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
            //�ʱ�ȭ
            isArrive = new bool[5];
            wayPointIndex = 0;
            dolly = cart.AutomaticDolly.Method as SplineAutoDolly.FixedSpeed;
            dolly.Speed = 0f;

            //������
            StartCoroutine(PlayStartSequence());
        }
        private void Update()
        {
            //���� ����
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
        //��ǥ ���� ����
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

        //�÷��� �� ����
        private void GoToPlayScene()
        {
            //�ڷ�ƾ ����
            StopAllCoroutines();

            //����� ����
            AudioManager.Instance.StopBgm();

            //������ ����
            fader.FadeTo(loadToScene);
        }

        //���� ������
        IEnumerator PlayStartSequence()
        {
            isArrive[0] = true;

            fader.FadeStart();

            //�����
            AudioManager.Instance.PlayBgm("IntroBgm");

            yield return new WaitForSeconds(1f);

            animator.SetTrigger(aroundTrigger);
            yield return new WaitForSeconds(2f);

            //�̵�����
            wayPointIndex = 1;
            dolly.Speed = 0.5f;
        }

        //�̵��� ����Ʈ ���� ���� ������
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

            //�̵�����
            wayPointIndex++;
        }

        //���� ���� ���� ������
        IEnumerator PlayFinallySequence()
        {
            isArrive[wayPointIndex] = true;

            //�̵� ����
            dolly.Speed = 0f;

            //��� ����Ʈ ����
            theShedLight.SetActive(false);

            yield return new WaitForSeconds(2f);

            //����� ����
            AudioManager.Instance.StopBgm();

            //������ ����
            fader.FadeTo(loadToScene);
        }
        #endregion
    }

}
