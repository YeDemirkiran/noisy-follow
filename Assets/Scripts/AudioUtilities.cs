using UnityEngine;

public class AudioUtilities
{         
    public static float GetLoudnessFromAudio(AudioClip clip, int clipPosition, int sampleWindow = 64)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            startPosition = 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);


        float totalLoudnessSquare = 0f;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudnessSquare += waveData[i] * waveData[i];
        }

        float rms = Mathf.Sqrt(totalLoudnessSquare / sampleWindow);
        return rms;
    }
}