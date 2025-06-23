using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class ProjectileStandard : ProjectileBase
    {
        #region Variables
        //����
        private ProjectileBase projectileBase;  //�ڽ��� �θ�Ŭ���� ��ü
        [SerializeField]
        private float maxLifeTime = 5f;         //�߻�ü ������ Ÿ��

        //�̵�
        [SerializeField]
        private float speed = 20f;
        //�߷� ����
        [SerializeField]
        private float gravityDown = 0f;        

        public Transform root;      //�߻�ü�� ��ġ
        public Transform tip;      //�߻�ü�� �Ӹ� ��ġ

        private Vector3 lastRootPosition;       //�߻�ü�� ���� �����ӿ����� ��ġ
        private Vector3 velocity;               //�߻�ü�� �ӵ�

        private float shootTime;       
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            projectileBase = GetComponent<ProjectileBase>();
            projectileBase.OnShoot += OnShoot;

            //���� �� ������ Ÿ�� �� ų
            Destroy(gameObject, maxLifeTime);
        }
        private void Update()
        {
            //�̵�
            transform.position += velocity * Time.deltaTime;

            //�߷�
            if(gravityDown > 0f)
            {
                velocity += Vector3.down * gravityDown * Time.deltaTime;
            }

            //
            lastRootPosition = root.position;
        }
        #endregion

        #region Custom Method
        private new void OnShoot()
        {
            //�ʱ�ȭ
            velocity = transform.forward * speed;
            transform.position += projectileBase.InheritedMuzzleVelocity * Time.deltaTime;

            lastRootPosition = root.position;


        }
        #endregion
    }

}
