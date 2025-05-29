using UnityEngine;

namespace MyFps
{
    public class PickUpItem : MonoBehaviour
    {
        #region Variables
        //아이템 연출
        [SerializeField]
        private float rotateSpeed = 5f;     //회전

        [SerializeField]
        private float verticalBobFrequency = 1f;    //위 아래 이동 속도

        [SerializeField]
        private float bobingAmount = 1f;

        private Vector3 startPos;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            startPos = this.transform.position;
        }
        private void Update()
        {
            //위 아래 이동
            float bobingAnimationPhase = Mathf.Sin(Time.time * verticalBobFrequency) * bobingAmount;
            transform.position = startPos + Vector3.up * bobingAnimationPhase;

            //아이템 회전
            this.transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {            
            if (other.tag == "Player")
            {
                if (OnPickUp())
                {                    
                    Destroy(this.gameObject);
                }                
            }
        }
        #endregion

        #region Custom Method
        protected virtual bool OnPickUp()
        {
            return true;
        }
        #endregion
    }

}
