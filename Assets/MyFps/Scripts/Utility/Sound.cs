using UnityEngine;

namespace MyFps
{
    [System.Serializable]
    public class Sound
    {
        #region Variables
        public string name;

        //Audio Source ������Ʈ�� �Ӽ�
        public AudioClip clip;      //���� Ŭ�� ���ҽ�

        [Range(0f, 1f)]
        public float voluem;    //���� ����

        [Range(0.1f, 3f)]
        public float pitch; //���� ����ӵ�

        public bool loop;   //���� �ݺ� ���� ����

        //���� �Ӽ����� ����� ����� �ҽ�
        public AudioSource audioSource;
        #endregion
    }

}
