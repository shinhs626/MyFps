using UnityEngine;

namespace Unity.FPS.Game
{
    //����� �÷��� �ϴ� Ŭ����
    public class AudioUtility
    {
        #region Variables

        #endregion

        #region Custom Method
        //���� ������Ʈ �����Ͽ� �����ϴ� ȿ���� �÷����ϴ� �Լ�
        public static void CreateSFX(AudioClip clip, Vector3 point, float spatialBlen,float rolloffDistanceMin = 1f)
        {
            GameObject impactSfxInstance = new GameObject();        //���̶�Űâ���� �� ������Ʈ �����
            impactSfxInstance.transform.position = point;   //��ġ ����

            AudioSource audioSource = impactSfxInstance.AddComponent<AudioSource>();    //���� ������ ���� ������Ʈ�� AudioSource ������Ʈ �߰�
            audioSource.clip = clip;    //�÷����� ����� Ŭ��
            audioSource.spatialBlend = spatialBlen; //3D ���� ȿ�� ����
            audioSource.minDistance = rolloffDistanceMin;   //3D ���� ȿ�� �ּ� �Ÿ�
            audioSource.Play();

            //������� �÷��� �� ų
            TimeSelfDestruct timeSelfDestruct = impactSfxInstance.AddComponent<TimeSelfDestruct>();
            timeSelfDestruct.lifeTime = clip.length;
        }
        #endregion
    }

}
