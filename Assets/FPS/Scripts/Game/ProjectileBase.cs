using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    //Projectile Ŭ�������� �θ� Ŭ����(�߻� Ŭ����)
    public abstract class ProjectileBase : MonoBehaviour
    {
        #region Variables
        //�߻� �� ��� �� �Լ��� ȣ���ϴ� �̺�Ʈ �Լ�
        public UnityAction OnShoot;
        #endregion

        #region Property
        public GameObject Owner { get; private set; }   //�߻��� ������ ����

        public Vector3 InitialPosition { get; private set; }    //�߻� �� ���� �ʱ� ��ġ
        public Vector3 InitialDirection { get; private set; }   //�߻� �� ���� �ʱ� �� ����
        public Vector3 InheritedMuzzleVelocity { get; private set; }    //�߻� �� �ѱ��� �̵� �ӵ�
        public float InitialCharge { get; private set; }    //�� Ÿ���� ���� ������
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        //
        public void Shoot(WeaponController controller)
        {
            Owner = controller.Owner;
            InitialPosition = this.transform.position;
            InitialDirection = this.transform.forward;
            InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            InitialCharge = controller.CurrentCharge;

            //�߻� �� ��ϵ� �Լ����� ȣ��
            OnShoot?.Invoke();
        }
        #endregion
    }

}
