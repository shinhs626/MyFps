using UnityEngine;
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

        //�ó����� ��� ó��
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequence = "I need get out of here";
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
            thePlayer.SetActive(false);

            //1. ���̵��� ���� (1�� ����� ���ε��� ȿ��)
            fader.FadeStart(1f);

            //2.ȭ�� �ϴܿ� �ó����� �ؽ�Ʈ ȭ�� ���(3��)
            sequenceText.text = sequence;

            //3. 3���Ŀ� �ó����� �ؽ�Ʈ ��������
            yield return new WaitForSeconds(3f);
            sequenceText.text = "";

            //4.�÷��� ĳ���� Ȱ��ȭ
            thePlayer.SetActive(true);
        }
        #endregion
    }
}