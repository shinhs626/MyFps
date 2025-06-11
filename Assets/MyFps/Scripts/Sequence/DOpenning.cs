using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MyFps
{
    public class DOpenning : MonoBehaviour
    {
        #region Variables
        //�÷��̾� ������Ʈ
        public GameObject thePlayer;
        //���̴� ��ü
        public SceneFader fader;

        //�ó����� ��� ó��
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequenceTextA = "Again...";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            /*
            //���� ������(�� ��ȣ) ����
            int sceneNumber = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("SaveScene" + sceneNumber);
            PlayerPrefs.SetInt("SaveScene", sceneNumber);
            */
            //File System
            PlayerDataManager.Instance.SceneNumber = SceneManager.GetActiveScene().buildIndex;
            SaveLoad.SaveData();

            //Ŀ�� ����
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            //0.�÷��� ĳ���� �� Ȱ��ȭ
            //thePlayer.SetActive(false);
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            //1. ���̵��� ���� (1�� ����� ���ε��� ȿ��)
            fader.FadeStart();

            //2.ȭ�� �ϴܿ� �ó����� �ؽ�Ʈ ȭ�� ���(3��)
            sequenceText.text = sequenceTextA;

            yield return new WaitForSeconds(1f);

            sequenceText.text = "";

            AudioManager.Instance.PlayBgm("Bgm");

            //Todo : Cheating
            //���� 2 �� �� ����
            PlayerDataManager.Instance.Weapon = WeaponType.Pistol;
            //PlayerDataManager.Instance.AddAmmo(5);

            input.enabled = true;
        }
        #endregion
    }

}
