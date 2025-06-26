using System;
using UnityEngine;

namespace Unity.FPS.Game
{
    //Lerp �Լ� ���� ����ü

    //Float�� Lerp�� ���ϱ�
    [Serializable]
    public struct MinMaxFloat
    {
        public float Min;
        public float Max;

        //�Ű������� ���� ratio�� �ش��ϴ� float Lerp�� ��������
        public float GetValueRatio(float ratio)
        {
            return Mathf.Lerp(Min, Max, ratio);
        }
    }

    //Color�� Lerp�� ���ϱ�
    [Serializable]
    public struct MinMaxColor
    {
        [ColorUsage(true,true)] public Color Min;
        [ColorUsage(true,true)] public Color Max;

        //�Ű������� ���� ratio(0~1)�� �ش��ϴ� Color Lerp�� ��������
        public Color GetValueRatio(float ratio)
        {
            return Color.Lerp(Min, Max, ratio);
        }
    }

    //Vector�� Lerp�� ���ϱ�
    [Serializable]
    public struct MinMaxVector
    {
        public Vector3 Min;
        public Vector3 Max;

        //�Ű������� ���� ratio(0~1)�� �ش��ϴ� Vector3 Lerp�� ��������
        public Vector3 GetValueRatio(float ratio)
        {
            return Vector3.Lerp(Min, Max, ratio);
        }
    }
}
