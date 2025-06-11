using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MyFps
{
    public class SaveLoad
    {
        //�����ϱ�
        public static void SaveData()
        {
            //���� �̸�, ��� ����
            string path = Application.persistentDataPath + "/pData.arr";

            //������ �����͸� ����ȭ �غ�
            BinaryFormatter formatter = new BinaryFormatter();

            //���� ���� - FileMode.Create : �����ϸ� ���� �������� ������ ���� �����
            FileStream fs = new FileStream(path, FileMode.Create);

            //������ �����͸� �غ� : �����ڸ� ���� ���̺� ������ ����
            PlayData playData = new PlayData();
            Debug.Log(playData.sceneNumber);

            //�غ��� �����͸� ����ȭ ����
            formatter.Serialize(fs, playData);

            //���Ͽ� �����ϸ� �׻� ������ �ݾ��־�� �Ѵ�
            fs.Close();
        }

        //�������� - ����� �����͸� ��ȯ������ �޾ƿ´�
        public static PlayData LoadData()
        {
            //�������� ������
            PlayData playData = null;

            //���� �̸�, ��� ����
            string path = Application.persistentDataPath + "/pData.arr";

            //������ ��ο� ������ ���� �ִ��� ������ üũ
            if(File.Exists(path) == true)
            {
                //����ȭ �����͸� ������ �غ�
                BinaryFormatter formatter = new BinaryFormatter();

                //���� ����
                FileStream fs = new FileStream(path, FileMode.Open);

                //����ȭ �������� ���Ͽ� ����� �����͸� ����ȭ�ؼ� �����´�
                playData = formatter.Deserialize(fs) as PlayData;
            }
            else
            {
                Debug.Log("Not Found");
            }

            return playData;
        }
    }

}
