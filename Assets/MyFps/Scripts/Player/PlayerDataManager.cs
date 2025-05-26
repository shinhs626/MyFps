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

    //플레이어 데이터 관리 클래스 - 싱글톤(다음 씬에서 데이터 보존)
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        #region Variables
        private int ammoCount;
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
            //플레이어 데이터 초기화
            Weapon = WeaponType.None;
            ammoCount = 0;
        }
        #endregion

        #region Custom Method
        //ammo 저축 함수
        public void AddAmmo(int amount)
        {
            ammoCount += amount;
            Debug.Log("총알 장전 완료" + AmmoCount +"발");
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
            Debug.Log("총알 발사 완료" + AmmoCount+"발");
            return true;
        }
        #endregion
    }

}
