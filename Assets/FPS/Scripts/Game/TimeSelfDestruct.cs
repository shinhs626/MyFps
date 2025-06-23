using UnityEngine;

namespace Unity.FPS.Game
{
    public class TimeSelfDestruct : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float lifeTime = 5f;
        private float spawnTime;    //생성시간
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //생성되는 시간 저장
            spawnTime = Time.time;
        }
        private void Update()
        {
            //생성 후 라이프 타임이 지나면 
            if((spawnTime + lifeTime) <= Time.time)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Custom Method

        #endregion
    }

}
