using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    public class AgentController : MonoBehaviour
    {
        #region Variables
        //참조
        private NavMeshAgent agent;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            //마우스 클릭한 지점으로 Agent 이동
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPosition = RayToWorld();
                agent.SetDestination(worldPosition);
            }
        }
        #endregion

        #region Custom Method
        //월드 포지션 값 얻어오기 - Ray 이용
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
