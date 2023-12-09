using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneSelector : MonoBehaviour
{
    [SerializeField] Button refreshButton;
    TMP_Dropdown dropdown;
    MicrophoneController microphone;

    IEnumerator Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        
        while (MicrophoneController.Instance == null) yield return null;
        microphone = MicrophoneController.Instance;

        RefreshOptions(true);

        dropdown.onValueChanged.AddListener(delegate { microphone.ChooseMicrophone(dropdown.value); RefreshOptions(false); } );
        refreshButton.onClick.AddListener(() => RefreshOptions(false));
    }

    void RefreshOptions(bool changeMicrophone)
    {
        string[] devices = microphone.GetDevices();

        string selection = dropdown.itemText.text;

        if (dropdown.options.Count != devices.Length)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(devices.ToList());

            int i = 0;

            foreach (TMP_Dropdown.OptionData option in dropdown.options)
            {
                if (option.text == selection)
                {
                    dropdown.value = i;

                    if (changeMicrophone) 
                    {
                        microphone.ChooseMicrophone(dropdown.value);
                    }

                    return;
                }

                i++;
            }

            dropdown.value = 0;

            if (changeMicrophone)
            {
                microphone.ChooseMicrophone(dropdown.value);
            }

            Debug.Log("New devices found.");
        }
    }
}