using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtn : MonoBehaviour
{

    [SerializeField]
    Sprite soundOn;
    [SerializeField]
    Sprite soundOff;

    // Use this for initialization
    void Start()
    {

        UpdateAudioStatus(PlayerPrefsPersister.GetAudioStatus());
    }

    public void ToggleSound()
    {
        bool isOn = PlayerPrefsPersister.GetAudioStatus();

        if (!isOn)
        {
            isOn = true;
            PlayerPrefsPersister.SetAudioStatus(isOn.ToString());
        }
        else
        {
            isOn = false;
            PlayerPrefsPersister.SetAudioStatus(isOn.ToString());
        }
        UpdateAudioStatus(isOn);
    }

    void UpdateAudioStatus(bool status)
    {
        if (Sound.instance != null)
        {
            Sound.instance.GetComponent<AudioSource>().enabled = status;
            GetComponent<Image>().sprite = status ? soundOn : soundOff;
        }
    }

}
