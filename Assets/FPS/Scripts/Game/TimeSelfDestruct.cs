using UnityEngine;

namespace Unity.FPS.Game
{
    public class TimeSelfDestruct : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float lifeTime = 5f;
        private float spawnTime;    //�����ð�
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //�����Ǵ� �ð� ����
            spawnTime = Time.time;
        }
        private void Update()
        {
            //���� �� ������ Ÿ���� ������ 
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
