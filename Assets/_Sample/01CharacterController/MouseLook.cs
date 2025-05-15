using UnityEngine;
using UnityEngine.InputSystem;

namespace MySample
{
    public class MouseLook : MonoBehaviour
    {
        #region Variables
        //참조
        public Transform cameraTrans;   //카메라 오브젝트

        [SerializeField]
        private float sensivity = 100f;

        //카메라 회전
        private float rotateX;

        //마우스 입력(위치,값)
        private Vector2 inputLook;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //마우스 포지션 좌우 입력 받아 플레이어 좌우 회전
            //float mouseX = Input.GetAxis("Mouse X") * sensivity;
            this.transform.Rotate(Vector3.up * Time.deltaTime * inputLook.x * sensivity);

            //마우스 포지션 위아래 입력 받아 카메라 위 아래 회전
            //float mouseY = Input.GetAxis("Mouse Y") * sensivity * -1;
            rotateX -= inputLook.y * Time.deltaTime * sensivity;
            rotateX = Mathf.Clamp(rotateX, -90f, 40f);
            cameraTrans.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
        }
        #endregion

        #region Custom Method
        public void OnLook(InputAction.CallbackContext context)
        {
            inputLook = context.ReadValue<Vector2>();
        }
        #endregion
    }

}
