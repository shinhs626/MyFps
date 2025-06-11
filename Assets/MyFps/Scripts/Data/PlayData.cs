using System;
using UnityEngine;

namespace MyFps
{
    [Serializable]
    public class PlayData
    {
        public int sceneNumber;
        public int ammoCount;
        public float playerHealth;

        //etc....

        //������ : ���� �ʱ�ȭ
        public PlayData()
        {
            sceneNumber = PlayerDataManager.Instance.SceneNumber;
            ammoCount = PlayerDataManager.Instance.AmmoCount;
            playerHealth = PlayerDataManager.Instance.PlayerHealth;
        }
    }
}

