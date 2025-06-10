using UnityEngine;

namespace MyFps
{
    //���� �Ӽ� �����͸� �����ϴ� ����ȭ�� Ŭ����
    [System.Serializable]
    public class Sound
    {
        //���� �Ӽ� ������ �̸�
        public string name;

        //AudioSource ������Ʈ�� �Ӽ�
        public AudioClip clip;  //���� Ŭ�� ���ҽ�
        [Range(0f, 1f)]
        public float volume;    //���� ���� 0~1
        [Range(0.1f, 3f)]
        public float pitch;     //���� ����ӵ� 
        public bool loop;       //���� �ݺ� ���� ����

        //���� �Ӽ����� ����� ������ҽ�
        public AudioSource source;
    }
}