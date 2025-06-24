using UnityEngine;

namespace Unity.FPS.Game
{
    //오디오 플레이 하는 클래스
    public class AudioUtility
    {
        #region Variables

        #endregion

        #region Custom Method
        //게임 오브젝트 생성하여 지정하는 효과음 플레이하는 함수
        public static void CreateSFX(AudioClip clip, Vector3 point, float spatialBlen,float rolloffDistanceMin = 1f)
        {
            GameObject impactSfxInstance = new GameObject();        //하이라키창에서 빈 오브젝트 만들기
            impactSfxInstance.transform.position = point;   //위치 지정

            AudioSource audioSource = impactSfxInstance.AddComponent<AudioSource>();    //새로 생성한 게임 오브젝트에 AudioSource 컴포넌트 추가
            audioSource.clip = clip;    //플레이할 오디오 클립
            audioSource.spatialBlend = spatialBlen; //3D 사운드 효과 설정
            audioSource.minDistance = rolloffDistanceMin;   //3D 사운드 효과 최소 거리
            audioSource.Play();

            //샤ㅏ운드 플레이 후 킬
            TimeSelfDestruct timeSelfDestruct = impactSfxInstance.AddComponent<TimeSelfDestruct>();
            timeSelfDestruct.lifeTime = clip.length;
        }
        #endregion
    }

}
