using UnityEngine;
namespace My3D
{
    public class CharacterAniTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        [SerializeField] private bool isWalk;
        [SerializeField] private bool isRun;

        private float velocity;
        //가속도 값
        [SerializeField]private float accelate = 4f;
        #endregion

        #region Property
        public bool IsWalk
        {
            get
            {
                return isWalk;
            }
            set
            {
                isWalk = value;
                animator.SetBool("IsWalk", value);
            }
        }
        public bool IsRun
        {
            get
            {
                return isRun;
            }
            set
            {
                isRun = value;
                animator.SetBool("IsRun", value);
            }
        }
        public float Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
                animator.SetFloat("Velocity", value);
            }
        }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            animator = this.GetComponent<Animator>();
            velocity = animator.GetFloat("Velocity");
        }
        private void Update()
        {
            IsWalk = Input.GetKey(KeyCode.W);
            IsRun = Input.GetKey(KeyCode.LeftShift);

            //if (IsWalk)
            //{
            //    Velocity += Time.deltaTime * accelate;
            //}
            //else
            //{
            //    Velocity -= Time.deltaTime * accelate;
            //}

           
            if(IsWalk && IsRun)
            {
                Velocity += Time.deltaTime * accelate;
            }
            else if(IsWalk)
            {
                Velocity += Time.deltaTime * accelate;
                if(Velocity > 4f)
                {
                    Velocity -= Time.deltaTime * accelate;
                }
                else
                {
                    Velocity += Time.deltaTime * accelate;
                }
            }
            else
            {
                Velocity -= Time.deltaTime * accelate;
            }
            Velocity = Mathf.Clamp(Velocity, 0f, 8f);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
            }
        }
        #endregion
    }

}

/*
1. w키를 누르면 걷기 애니메이션 플레이
2. w키를 누르고, Shift키 까지 누르면 뛰기 애니메이션 플레이
*/