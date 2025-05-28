using Sample.Generic;
using UnityEngine;

namespace MyFps
{
    public class AudioManager : Singleton<AudioManager>
    {
        #region Variables
        //���� ������ ���
        public Sound[] sounds;

        //����� �̸�
        private string bgmSound = "";
        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            //���� ������ ����
            foreach(var s in sounds)
            {
                //AudioSource ������Ʈ �߰�
                s.audioSource = this.gameObject.AddComponent<AudioSource>();

                s.audioSource.clip = s.clip;
                s.audioSource.volume = s.voluem;
                s.audioSource.pitch = s.pitch;
                s.audioSource.loop = s.loop;

                s.audioSource.playOnAwake = false;
            }
        }
        #endregion

        #region Custom Method
        //���� �÷���
        public void Play(string name)
        {
            Sound sound = null;

            //���� ��Ͽ��� ���� �̸��� ���� ã��
            foreach(var s in sounds)
            {
                if(s.name == name)
                {
                    sound = s;
                    break;
                }
            }

            //ã�Ҵ��� üũ
            if(sound == null)
            {
                Debug.Log("���带 ã�� �� �����ϴ�" + name);
                return;
            }
            sound.audioSource.Play();
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
                Debug.Log("���带 ã�� �� �����ϴ�" + name);
                return;
            }
            sound.audioSource.Stop();
        }

        //����� �÷���
        public void PlayBgm(string name)
        {
            //����� üũ
            if(bgmSound == name)
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
                Debug.Log("���带 ã�� �� �����ϴ�" + name);
                return;
            }
            sound.audioSource.Stop();
        }
        #endregion
    }

}
