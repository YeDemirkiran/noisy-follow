using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.isOn = PlayerData.showCalibrationAtStart;

        toggle.onValueChanged.AddListener( delegate { CalibrationBool(toggle.isOn); } );
    }

    void CalibrationBool(bool boo)
    {
        PlayerData.showCalibrationAtStart = boo;
    }
}