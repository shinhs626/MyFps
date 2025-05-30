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

    //���� ������ enum �� ����
    public enum PuzzleKey
    {
        ROOM01_KEY,
        MAXKEY,     //���� ������ ����
    }

    //�÷��̾� ������ ���� Ŭ���� - �̱���(���� ������ ������ ����)
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        #region Variables
        private int ammoCount;

        private bool[] hasKeys;      //���� ������ ���� ���� üũ
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
            InitPlayerData();
        }
        #endregion

        #region Custom Method
        private void InitPlayerData()
        {
            //�÷��̾� ������ �ʱ�ȭ
            Weapon = WeaponType.None;
            ammoCount = 0;

            //���� ������ ���� : ���� ������ ������ŭ bool�� ��Ҽ� ����
            hasKeys = new bool[(int)PuzzleKey.MAXKEY];
        }

        //ammo ���� �Լ�
        public void AddAmmo(int amount)
        {
            ammoCount += amount;
            //Debug.Log("�Ѿ� ���� �Ϸ�" + AmmoCount +"��");
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
            //Debug.Log("�Ѿ� �߻� �Ϸ�" + AmmoCount+"��");
            return true;
        }

        //���� ������ ȹ�� - �Ű������� ����Ű Ÿ�� �޴´�
        public void  GainPuzzleKey(PuzzleKey key)
        {
            hasKeys[(int)key] = true;
        }
        //���� ������ ���� ���� üũ - �Ű������� 
        public bool HasPuzzleKey(PuzzleKey key)
        {
            return hasKeys[(int)key];
        }
        #endregion
    }

}
