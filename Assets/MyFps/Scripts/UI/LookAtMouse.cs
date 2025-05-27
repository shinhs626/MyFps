using UnityEngine;

namespace MyFps
{
    public class LookAtMouse : MonoBehaviour
    {
        #region Variables
        //���콺 �����Ͱ� ����Ű�� ���� ������ ��
        private Vector3 worldPosition;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //���� ������ �� ������
            worldPosition = RayToWorld();
            //worldPosition = ScreenToWolrd();

            //���� ������ �� �ٶ󺸱�
            transform.LookAt(worldPosition);
        }
        #endregion

        #region Custom Method
        //���� ������ �� ������ - ���콺�� ��ġ ���� �̿��Ͽ�
        private Vector3 ScreenToWolrd()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Vector3.zero;

            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 2f));

            return worldPos;
        }

        //���� ������ �� ������ - Ray �̿�
        private Vector3 RayToWorld()
        {
            Vector3 worldPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                worldPos = hit.point;
            }

            return worldPos;
        }
        #endregion
    }

}
