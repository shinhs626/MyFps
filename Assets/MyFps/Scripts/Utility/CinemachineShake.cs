using Sample.Generic;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace MyFps
{
    public class CinemachineShake : Singleton<CinemachineShake>
    {
        #region Variables
        //참조
        private CinemachineBasicMultiChannelPerlin channelPerlin;

        private bool isShake;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            channelPerlin = this.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
        #endregion

        #region Custom Method
        //카메라 흔들기
        //aGain : 흔들림 크기
        //fGain : 흔들림 속도
        //shakeTime : 흔들리는 시간
        public void Shake(float aGain, float fGain, float shakeTime)
        {
            if (isShake)
                return;

            StartCoroutine(StartShake(aGain,fGain,shakeTime));
        }

        IEnumerator StartShake(float aGain, float fGain, float shakeTime)
        {
            channelPerlin.AmplitudeGain = aGain;
            channelPerlin.FrequencyGain = fGain;

            isShake = true;

            yield return new WaitForSeconds(shakeTime);

            channelPerlin.AmplitudeGain = 0f;
            channelPerlin.FrequencyGain = 0f;

            isShake = false;
        }
        #endregion
    }

}
