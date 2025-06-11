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
        LEFT_EYE,
        RIGHT_EYE,
        MAXKEY,     //���� ������ ����
    }

    //�÷��̾� ������ ���� Ŭ���� - �̱���(���� ������ ������ ����)
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        #region Variables
        private int sceneNumber;    //�� �ѹ�
        private int ammoCount;      //������ źȯ ����
        private float playerHealth; //�÷��̾� ü��

        private bool[] hasKeys;      //���� ������ ���� ���� üũ

        [SerializeField]
        private float maxPlayerHealth = 20f;
        #endregion

        #region Property
        public WeaponType Weapon { get; set; }

        //�÷��� ���� �� ���� ��ȣ
        public int SceneNumber
        {
            get
            {
                return sceneNumber;
            }
            set
            {
                sceneNumber = value;
            }
        }

        //źȯ ���� �����ϴ� �б� ���� ������Ƽ
        public int AmmoCount
        {
            get
            {
                return ammoCount;
            }
            set
            {
                ammoCount = value;
            }
        }

        public float PlayerHealth
        {
            get
            {
                return playerHealth;
            }
            set
            {
                playerHealth = value;
            }
        }
        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            InitPlayerData();
        }
        #endregion

        #region Custom Method
        public void InitPlayerData(PlayData pData = null)
        {
            if(pData != null)
            {
                sceneNumber = pData.sceneNumber;
                ammoCount = pData.ammoCount;
                playerHealth = pData.playerHealth;
            }
            else
            {
                //�÷��̾� ������ �ʱ�ȭ
                sceneNumber = -1;
                ammoCount = 0;
                playerHealth = maxPlayerHealth;
            }
            Weapon = WeaponType.None;

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
