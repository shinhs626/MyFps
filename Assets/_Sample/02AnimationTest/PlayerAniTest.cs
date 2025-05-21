using UnityEngine;

namespace MySample
{
    public class PlayerAniTest : MonoBehaviour
    {
        #region Variables
        //����
        private Animator animator;

        //�̵�
        [SerializeField]
        private float moveSpeed = 5f;

        //��ǲ 
        private float moveX;
        private float moveY;

        //�ִϸ��̼� �Ķ����
        [SerializeField]
        private string moveMode = "MoveMode";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            animator = this.GetComponent<Animator>();

        }


        private void Update()
        {
            //��ǲ
            moveX = Input.GetAxis("Horizontal");       //ad, �¿� ȭ��ǥ
            moveY = Input.GetAxis("Vertical");         //ws, ���Ʒ� ȭ��ǥ  -1 ~ 1

            //����
            Vector3 dir = new Vector3(moveX, 0f, moveY);
            transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);

            //�ִϸ��̼�
            //AnimationStateTest();
            AnimationBlendTest();
        }

        //
        private void AnimationBlendTest()
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }

        //2. �յ��¿� �̵��� �յ��¿� �ִϸ��̼� �÷��� �����ش�
        //3. �̵��� ���� ������ ��� �ִϸ��̼��� �÷��� �Ѵ�
        private void AnimationStateTest()
        {
            if(moveX == 0f && moveY == 0f)
            {
                animator.SetInteger(moveMode, 0);
            }
            else
            {
                //�յ��¿�
                if (moveX < 0)
                {
                    animator.SetInteger(moveMode, 3);
                }
                else if (moveX > 0)
                {
                    animator.SetInteger(moveMode, 4);
                }
                if (moveY > 0)
                {
                    animator.SetInteger(moveMode, 1);
                }
                else if (moveY < 0)
                {
                    animator.SetInteger(moveMode, 2);
                }
            }
        }
        #endregion
    }


}

/*
1. WASD �Է� �޾� �÷��̾� �յ��¿� �̵��ϰ� (old input)

2. �յ��¿� �̵��� �յ��¿� �ִϸ��̼� �÷��� �����ش�
3. �̵��� ���� ������ ��� �ִϸ��̼��� �÷��� �Ѵ�

4. �̵��ӵ� 5
*/

