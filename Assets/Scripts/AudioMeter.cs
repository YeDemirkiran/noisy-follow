using UnityEngine;
using UnityEngine.UI;

public class AudioMeter : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float refreshRateInSeconds = 0.05f, threshold, multiplier;

    MicrophoneController microphone;

    float timer = 0f;

    private void Start()
    {
        microphone = MicrophoneController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= refreshRateInSeconds)
        {
            timer = 0f;
            slider.value = Mathf.Lerp(0f, 1f, microphone.GetLoudnessFromMicrophone(threshold, multiplier));
        }
    }
}