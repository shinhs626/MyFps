using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MyFps
{
    public class PauseUI : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;

        public GameObject pauseUI;
        public PlayerInput playerInput;

        private string loadToScene = "MainMenu";
        #endregion

        #region Custom Method
        public void OnEsc(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Toggle();
            }
        }
        public void Toggle()
        {
            bool isAction = pauseUI.activeSelf;
            pauseUI.SetActive(!isAction);

            //���� �Ͻ����� ó��
            if (!isAction)  //���� ����
            {
                //playerInput.enabled = false;    //���� �Ҵ�
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else    //���� ����
            {
                //playerInput.enabled = true;     //���� ����
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        public void MainMenu()
        {
            //Debug.Log("���� �޴��� �̵�");
            fader.FadeTo(loadToScene);
        }
        #endregion
    }

}
