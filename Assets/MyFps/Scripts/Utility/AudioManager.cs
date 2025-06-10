using Sample.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace MyFps
{
    //���带 �����ϴ� �̱��� Ŭ����
    public class AudioManager : Singleton<AudioManager>
    {
        #region Variables
        //���� ������ ���
        public Sound[] sounds;

        //����� �̸�
        private string bgmSound = "";

        //AudioMixer
        public AudioMixer audioMixer;
        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            //AudioMixer���� ����� �׷� ��������
            AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");

            //���� ������ ����
            foreach (var s in sounds)
            {
                //AudioSource ������Ʈ �߰� ����
                s.source = this.gameObject.AddComponent<AudioSource>();

                //AudioSource ������Ʈ ������ ����
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

                s.source.playOnAwake = false;

                if (s.source.loop)
                {
                    s.source.outputAudioMixerGroup = audioMixerGroups[1];
                }
                else
                {
                    s.source.outputAudioMixerGroup = audioMixerGroups[2];
                }
            }
        }
        #endregion

        #region Custom Method
        //���� �÷���
        public void Play(string name)
        {
            Sound sound = null;
            //���� ��Ͽ��� ���� �̸��� ���� ã��
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    break;
                }
            }

            //ã�Ҵ��� üũ
            if (sound == null)
            {
                Debug.Log("Cannot Find " + name + " Sound");
                return;
            }

            sound.source.Play();
        }

        //���� ����
        public void Stop(string name)
        {
            Sound sound = null;

            //���� ��Ͽ��� ���� �̸��� ���� ã��
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    break;
                }
            }

            //ã�Ҵ��� üũ
            if (sound == null)
            {
                Debug.Log("Cannot Find " + name + " Sound");
                return;
            }

            sound.source.Stop();
        }

        //����� �÷���
        public void PlayBgm(string name)
        {
            //����� �̸� üũ
            if (bgmSound == name)
            {
                return;
            }

            //���� �÷��� �ǰ� �ִ� ����� ����
            Stop(bgmSound);

            //����� �÷���
            Sound sound = null;

            //���� ��Ͽ��� ���� �̸��� ���� ã��
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    //����� �̸� ����
                    bgmSound = name;
                    break;
                }
            }

            //ã�Ҵ��� üũ
            if (sound == null)
            {
                Debug.Log("Cannot Find " + name + " Bgm Sound");
                return;
            }

            sound.source.Stop();
        }

        public void StopBgm()
        {
            //����� ����
            Stop(bgmSound);
        }
        #endregion
    }
}
