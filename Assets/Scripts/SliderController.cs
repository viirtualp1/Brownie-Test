using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderController : MonoBehaviour
{
    public AudioMixer Mixer;

    public Slider slider_m;
    public float oldVolume_m;

    public Slider slider_v;
    public float oldVolume_v;

    private void Start()
    {
        oldVolume_m = slider_m.value;
        if (!PlayerPrefs.HasKey("volume_m")) slider_m.value = 0;
        else slider_m.value = PlayerPrefs.GetFloat("volume_m");

        oldVolume_v = slider_v.value;
        if (!PlayerPrefs.HasKey("volume_v")) slider_v.value = 7;
        else slider_v.value = PlayerPrefs.GetFloat("volume_v");
    }

    private void Update()
    {
        if(oldVolume_m != slider_m.value)
        {
            PlayerPrefs.SetFloat("volume_m", slider_m.value);
            PlayerPrefs.Save();
            //Mixer.SetFloat("musicVol", slider.value);
            Mixer.SetFloat("musicVol", PlayerPrefs.GetFloat("volume_m"));
            oldVolume_m = slider_m.value;
        }

        if(oldVolume_v != slider_v.value)
        {
            PlayerPrefs.SetFloat("volume_v", slider_v.value);
            PlayerPrefs.Save();
            Mixer.SetFloat("voiceVol", PlayerPrefs.GetFloat("volume_v"));
            oldVolume_v = slider_v.value;
        }

    }

}