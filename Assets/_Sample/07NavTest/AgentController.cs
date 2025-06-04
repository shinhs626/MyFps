using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    public class AgentController : MonoBehaviour
    {
        #region Variables
        //����
        private NavMeshAgent agent;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            //���콺 Ŭ���� �������� Agent �̵�
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPosition = RayToWorld();
                agent.SetDestination(worldPosition);
            }
        }
        #endregion

        #region Custom Method
        //���� ������ �� ������ - Ray �̿�
        private Vector3 RayToWorld()
        {
            Vector3 worldPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                worldPos = hit.point;
            }

            return worldPos;
        }
        #endregion
    }

}
