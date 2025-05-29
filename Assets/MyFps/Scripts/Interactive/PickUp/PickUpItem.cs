using UnityEngine;

namespace MyFps
{
    public class PickUpItem : MonoBehaviour
    {
        #region Variables
        //������ ����
        [SerializeField]
        private float rotateSpeed = 5f;     //ȸ��

        [SerializeField]
        private float verticalBobFrequency = 1f;    //�� �Ʒ� �̵� �ӵ�

        [SerializeField]
        private float bobingAmount = 1f;

        private Vector3 startPos;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //�ʱ�ȭ
            startPos = this.transform.position;
        }
        private void Update()
        {
            //�� �Ʒ� �̵�
            float bobingAnimationPhase = Mathf.Sin(Time.time * verticalBobFrequency) * bobingAmount;
            transform.position = startPos + Vector3.up * bobingAnimationPhase;

            //������ ȸ��
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
