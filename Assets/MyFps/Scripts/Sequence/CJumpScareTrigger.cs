using System.Collections;
using UnityEngine;

namespace MyFps
{
    public class CJumpScareTrigger : MonoBehaviour
    {
        #region Variables
        public Animator animator;

        //문 열리는 사운드
        public AudioSource doorBang;
        //적 등장 사운드
        public AudioSource jumpScare;
        //Enemy 오브젝트
        public GameObject enemyZozi;

        //애니메이션 파라미터
        private string isOpen = "IsOpen";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                //트리거 해제
                this.GetComponent<BoxCollider>().enabled = false;

                StartCoroutine(SequencePlay());
            }
        }
        #endregion
        #region Custom Method
        //트리거 연출 구현
        IEnumerator SequencePlay()
        {
            animator.SetBool(isOpen, true);

            doorBang.Play();

            yield return new WaitForSeconds(1f);

            //
            enemyZozi.SetActive(true);

            jumpScare.Play();

            Zozi zozi = enemyZozi.GetComponent<Zozi>();

            if (zozi)
            {
                zozi.ChangeState(ZoziState.Z_Walk);
            }
        }
        #endregion
    }

}
