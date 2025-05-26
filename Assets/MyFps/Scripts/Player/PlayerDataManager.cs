using Sample.Generic;
using UnityEngine;

namespace MyFps
{
    //���� ���� Ÿ�� Enum
    public enum WeaponType
    {
        None,
        Pistol
    }

    //�÷��̾� ������ ���� Ŭ���� - �̱���(���� ������ ������ ����)
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        #region Variables
        private int ammoCount;
        #endregion

        #region Property
        public WeaponType Weapon { get; set; }

        //źȯ ���� �����ϴ� �б� ���� ������Ƽ
        public int AmmoCount
        {
            get
            {
                return ammoCount;
            }
        }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //�÷��̾� ������ �ʱ�ȭ
            Weapon = WeaponType.None;
            ammoCount = 0;
        }
        #endregion

        #region Custom Method
        //ammo ���� �Լ�
        public void AddAmmo(int amount)
        {
            ammoCount += amount;
            Debug.Log("�Ѿ� ���� �Ϸ�" + AmmoCount +"��");
        }

        //ammo ��� �Լ�
        public bool UseAmmo(int amount)
        {
            if (ammoCount < amount)
            {
                Debug.Log("Need Reload");
                return false;
            }
                

            ammoCount -= amount;
            Debug.Log("�Ѿ� �߻� �Ϸ�" + AmmoCount+"��");
            return true;
        }
        #endregion
    }

}
