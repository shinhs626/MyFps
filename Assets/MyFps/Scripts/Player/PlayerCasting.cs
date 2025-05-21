using UnityEngine;

namespace MyFps
{
    //�÷��̾�� ���鿡 �ִ� ������Ʈ(�ݶ��̴�) ���� �Ÿ��� ���ϴ� Ŭ����
    //RayCast�� �̿��Ѵ�
    public class PlayerCasting : MonoBehaviour
    {
        #region Variables
        //Ÿ�ٱ����� �Ÿ�
        public static float distanceFromTarget;
        public float toTarget;      //�ν�����â ������
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //�ʱ�ȭ
            distanceFromTarget = Mathf.Infinity;
            toTarget = distanceFromTarget;
        }

        private void Update()
        {
            //���̸� ��� �Ÿ� ���ϱ�
            RaycastHit hit; //����hit ������ �����ϴ� ����
            
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                distanceFromTarget = hit.distance;
                toTarget = distanceFromTarget;
            }

        }

        //���� ����� �׸���, ���� 100�� ���� ����� �׸���
        private void OnDrawGizmosSelected()
        {
            RaycastHit hit;
            float maxDistance = 100f;
            bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance);

            Gizmos.color = Color.red;
            if(isHit)
            {
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            }
            else
            {
                Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
            }
        }
        #endregion
    }
}