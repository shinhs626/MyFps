using Sample.Generic;
using UnityEngine;

namespace MyFps
{
    //장착 무기 타입 Enum
    public enum WeaponType
    {
        None,
        Pistol
    }

    //퍼즐 아이템 enum 값 설정
    public enum PuzzleKey
    {
        ROOM01_KEY,
        MAXKEY,     //퍼즐 아이템 갯수
    }

    //플레이어 데이터 관리 클래스 - 싱글톤(다음 씬에서 데이터 보존)
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        #region Variables
        private int ammoCount;

        private bool[] hasKeys;      //퍼즐 아이템 소지 여부 체크
        #endregion

        #region Property
        public WeaponType Weapon { get; set; }

        //탄환 갯수 리턴하는 읽기 전용 프로퍼티
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
            //플레이어 데이터 초기화
            Weapon = WeaponType.None;
            ammoCount = 0;

            //퍼즐 아이템 설정 : 퍼즐 아이템 갯수만큼 bool형 요소수 생성
            hasKeys = new bool[(int)PuzzleKey.MAXKEY];
        }

        //ammo 저축 함수
        public void AddAmmo(int amount)
        {
            ammoCount += amount;
            //Debug.Log("총알 장전 완료" + AmmoCount +"발");
        }

        //ammo 사용 함수
        public bool UseAmmo(int amount)
        {
            if (ammoCount < amount)
            {
                Debug.Log("Need Reload");
                return false;
            }
                

            ammoCount -= amount;
            //Debug.Log("총알 발사 완료" + AmmoCount+"발");
            return true;
        }

        //퍼즐 아이템 획득 - 매개변수로 퍼즐키 타입 받는다
        public void  GainPuzzleKey(PuzzleKey key)
        {
            hasKeys[(int)key] = true;
        }
        //퍼즐 아이템 소지 여부 체크 - 매개변수로 
        public bool HasPuzzleKey(PuzzleKey key)
        {
            return hasKeys[(int)key];
        }
        #endregion
    }

}
