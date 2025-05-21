using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    public class MouseLook : MonoBehaviour
    {
        #region Variables
        //����
        public Transform cameraTrans;   //ī�޶� ��Ʈ

        //���콺 �Է°��� ������
        [SerializeField]
        private float sensivity = 100f;

        //ī�޶� ȸ��
        private float rotateX;

        //���콺 �Է�(��ġ) ��
        private Vector2 inputLook;

        //�׶��� üũ

        #endregion

        #region Unity Event Method
        private void Update()
        {
            //���콺 ������ �¿� �Է� �޾� - old Input 
            //float mouseX = Input.GetAxis("Mouse X") * sensivity;

            //�÷��̾� �¿� ȸ��
            this.transform.Rotate(Vector3.up * Time.deltaTime * inputLook.x * sensivity);

            //���콺 ������ ���Ʒ� �Է� �޾� ī�޶� ���Ʒ� ȸ�� - old Input
            //float mouseY = Input.GetAxis("Mouse Y") * sensivity;

            //ī�޶� ���Ʒ� ȸ��
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
