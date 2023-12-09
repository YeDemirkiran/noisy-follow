using UnityEngine;
using UnityEngine.UI;

public class AudioMeter : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float refreshRateInSeconds = 0.05f, threshold, multiplier;

    MicrophoneController microphone;

    float timer = 0f;

    void Start()
    {
        microphone = MicrophoneController.Instance;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= refreshRateInSeconds)
        {
            float loudness = microphone.GetLoudnessFromMicrophone(threshold, multiplier);

            timer = 0f;
            slider.value = Mathf.Lerp(0f, 1f, loudness);
        }
    }
}