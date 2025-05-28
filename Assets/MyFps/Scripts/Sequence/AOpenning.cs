using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

namespace MyFps
{
    //�÷��� �� ������ ����
    public class AOpenning : MonoBehaviour
    {
        #region Variables
        //�÷��̾� ������Ʈ
        public GameObject thePlayer;
        //���̴� ��ü
        public SceneFader fader;
        public AudioSource bgm;

        public AudioSource line01;
        public AudioSource line02;

        //�ó����� ��� ó��
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequence01 = "Where am I?";
        [SerializeField]
        private string sequence02 = "I need get out of here";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //Ŀ�� ����
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //������ ���� ����
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        //������ ���� �ڷ�ƾ �Լ�
        IEnumerator SequencePlay()
        {
            //0.�÷��� ĳ���� �� Ȱ��ȭ
            //thePlayer.SetActive(false);
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            //1. ���̵��� ���� (1�� ����� ���ε��� ȿ��)
            fader.FadeStart(4f);

            //2.ȭ�� �ϴܿ� �ó����� �ؽ�Ʈ ȭ�� ���(3��)
            sequenceText.text = sequence01;
            line01.Play();

            //3. 3���Ŀ� �ó����� �ؽ�Ʈ ��������
            yield return new WaitForSeconds(4f);

            sequenceText.text = sequence02;
            line02.Play();

            yield return new WaitForSeconds(3f);
            sequenceText.text = "";

            bgm.Play();

            //4.�÷��� ĳ���� Ȱ��ȭ
            //thePlayer.SetActive(true);
            input.enabled = true;   
        }
        #endregion
    }
}