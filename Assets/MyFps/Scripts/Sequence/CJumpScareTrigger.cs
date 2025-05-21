using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;

namespace MyFps
{
    //�� ���� Ʈ���� ����
    public class CJumpScareTrigger : MonoBehaviour
    {
        #region Variables
        //�������� �ִϸ��̼�
        public Animator animator;

        //�������� ����
        public AudioSource doorBang;

        //�� ������Ʈ
        public GameObject enemy;

        //�� ���� ����
        public AudioSource jumpScare;

        //�ִϸ��̼� �Ķ����
        private string isOpen = "IsOpen";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            //�÷��̾� üũ
            if(other.tag == "Player")
            {
                //Ʈ���� ����
                this.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(SequencePlayer());
            }
        }
        #endregion

        #region Custom Method
        //Ʈ���� ���� ����
        IEnumerator SequencePlayer()
        {
            //���� ������
            animator.SetBool(isOpen, true);

            //���� ������ ����
            doorBang.Play();

            //�� ���� 
            enemy.SetActive(true);

            //1�� ������
            yield return new WaitForSeconds(1f);

            //�� ���� ���� �÷���
            jumpScare.Play();

            //�κ��� ���°� �ȱ� ���·� ����
            Robot robot = enemy.GetComponent<Robot>();
            if(robot)
            {
                robot.ChangeState(RobotState.R_Walk);
            }
        }
        #endregion
    }
}

