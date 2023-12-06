using UnityEngine;

public class MicrophoneController : MonoBehaviour
{
    public static MicrophoneController Instance { get; private set; }

    AudioSource audioSource;
    AudioClip microphoneRecord;

    [SerializeField] int sampleWindow = 16;

    void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = null;

        microphoneRecord = MicrophoneToAudioClip();
        audioSource.clip = microphoneRecord;
        //audioSource.Play();
    }

    public float GetLoudnessFromMicrophone(float threshold, float outputMultiplier)
    {
        float volume = AudioUtilities.GetLoudnessFromAudio(microphoneRecord, Microphone.GetPosition(Microphone.devices[0]), sampleWindow);

        if (volume <= threshold)
        {
            return 0f;
        }

        return outputMultiplier * volume;
    }

    AudioClip MicrophoneToAudioClip()
    {
        string deviceName = Microphone.devices[0];

        Debug.Log("Device name: " + deviceName);
        return Microphone.Start(deviceName, true, 5, AudioSettings.outputSampleRate);
    }
}