using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    Slider slider;
    [SerializeField] TMP_Text valueText;

    void Start()
    {
        slider = GetComponent<Slider>();

        slider.value = PlayerData.volumeMultiplier;
        valueText.text = PlayerData.volumeMultiplier.ToString("0.0");

        slider.onValueChanged.AddListener(delegate { ChangeVolumeMultiplier(slider.value); });
    }

    void ChangeVolumeMultiplier(float volume)
    {
        PlayerData.volumeMultiplier = volume;
        valueText.text = volume.ToString("0.0");
    }
}