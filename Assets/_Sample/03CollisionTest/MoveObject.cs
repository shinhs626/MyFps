using UnityEngine;
namespace MySample
{
    public class MoveObject : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody rb;

        //오브젝ㅌ에 주는 힘
        [SerializeField] private float movePower = 3f;

        //오브젝트 컬러 바꾸기
        private Renderer render;
        private Color originColor;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            rb = this.GetComponent<Rigidbody>();
            render = this.GetComponent<Renderer>();

            //초기화
            originColor = render.material.color;

            //오른쪽으로 3만큼 힘을 준다   -   일회성
            MoveRight();
        }
        private void Update()
        {
            
        }
        #endregion

        #region Custom Method
        public void MoveLeft()
        {
            rb.AddForce(Vector3.left * movePower, ForceMode.Impulse);
        }
        public void MoveRight()
        {
            rb.AddForce(Vector3.right * movePower, ForceMode.Impulse);
        }
        public void ChangeColor()
        {
            render.material.color = Color.red;
        }
        public void OriginColor()
        {
            render.material.color = originColor;
        }
        #endregion
    }

}
