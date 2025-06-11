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

            //게임 일시정지 처리
            if (!isAction)  //열린 상태
            {
                //playerInput.enabled = false;    //조작 불능
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else    //닫힌 상태
            {
                //playerInput.enabled = true;     //조작 가능
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        public void MainMenu()
        {
            //Debug.Log("메인 메뉴로 이동");
            fader.FadeTo(loadToScene);
        }
        #endregion
    }

}
