using UnityEngine;

public class MicrophoneController : MonoBehaviour
{
    public static MicrophoneController Instance { get; private set; }

    AudioSource audioSource;
    AudioClip microphoneRecord;

    [SerializeField] int sampleWindow = 16;

    string currentDeviceName;

    void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public float GetLoudnessFromMicrophone(float threshold, float outputMultiplier)
    {
        if (microphoneRecord == null) { return -1f; }

        float volume = AudioUtilities.GetLoudnessFromAudio(microphoneRecord, Microphone.GetPosition(Microphone.devices[0]), sampleWindow);

        if (volume <= threshold)
        {
            return 0f;
        }

        return outputMultiplier * volume;
    }

    public void ChooseMicrophone(int index)
    {
        EndMicrophone();

        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("No Microphone found!");
            return;
        }

        if (index < 0 || index >= Microphone.devices.Length)
        {
            Debug.Log("Device index out of bounds. Choosing the default.");
            currentDeviceName = Microphone.devices[0];

            StartMicrophone(false);
            return;
        }

        currentDeviceName = Microphone.devices[index];
        StartMicrophone(true);
    }

    public int GetCurrentMicrophoneIndex()
    {
        int i = 0;

        foreach (string device in Microphone.devices)
        {
            if (device == currentDeviceName)
            {
                return i;
            }

            i++;
        }

        Debug.Log("Microphone couldn't be found.");
        return -1;
    }

    public string[] GetDevices()
    {
        return Microphone.devices;
    }

    void StartMicrophone(bool play)
    {
        audioSource.clip = null;
        audioSource.Stop();

        microphoneRecord = MicrophoneToAudioClip();
        audioSource.clip = microphoneRecord;

        if (play)
        {
            while (!(Microphone.GetPosition(currentDeviceName) > 0)) { }

            audioSource.Play();
        }
    }

    void EndMicrophone()
    {
        audioSource.clip = null;
        microphoneRecord = null;
        audioSource.Stop();
        Microphone.End(currentDeviceName);
    }

    AudioClip MicrophoneToAudioClip()
    {
        Debug.Log("Device name: " + currentDeviceName);
        return Microphone.Start(currentDeviceName, true, 5, AudioSettings.outputSampleRate);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            EndMicrophone();
        }
        else
        {
            StartMicrophone(true);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            EndMicrophone();
        }
        else
        {
            StartMicrophone(true);
        }
    }
}