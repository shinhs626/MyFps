using UnityEngine;

namespace MySample
{
    public class SoundTest : MonoBehaviour
    {
        #region MyRegion
        private AudioSource audioSource;

        //Audio Source
        public AudioClip audioClip;
        public float volume = 10f;
        public float pitch = 1.0f;
        public bool loop;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            audioSource = this.GetComponent<AudioSource>();

            SoundOnShot();
        }
        #endregion

        #region Custom Method
        private void SoundOnShot()
        {
            //audioSource.Play();
            audioSource.PlayOneShot(audioClip);
        }
        private void SoundPlay()
        {
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.loop = loop;
        }
        #endregion
    }

}
