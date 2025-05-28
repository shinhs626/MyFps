using UnityEngine;

namespace MyFps
{
    [System.Serializable]
    public class Sound
    {
        #region Variables
        public string name;

        //Audio Source 컴포넌트의 속성
        public AudioClip clip;      //사운드 클립 리소스

        [Range(0f, 1f)]
        public float voluem;    //사운드 볼륨

        [Range(0.1f, 3f)]
        public float pitch; //사운드 재생속도

        public bool loop;   //사운드 반복 실행 여부

        //위의 속성값을 재생할 오디오 소스
        public AudioSource audioSource;
        #endregion
    }

}
