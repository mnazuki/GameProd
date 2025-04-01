using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    private void Start()
    {
        if (PlayerPrefs.HasKey("MainMusic"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
            SetElectronMusicVolume();
            SetSFXElectronVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MainMusic",volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    public void SetElectronMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("ElectronMusic", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ElectronMusic", volume);
    }

    public void SetSFXElectronVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFXElectron", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXElectron", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MainMusic");
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        musicSlider.value = PlayerPrefs.GetFloat("ElectronMusic");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXElectron");

        SetSFXVolume();
        SetMusicVolume();
        SetElectronMusicVolume();
        SetSFXElectronVolume();
    }
}
