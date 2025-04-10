using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer myMixer;

    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [Header("Mixer Identifier")]
    [Tooltip("Unique identifier to create unique keys for this scene's mixer (e.g. Main, Electron)")]
    [SerializeField] private string mixerIdentifier = "Default";

    // Construct unique keys for PlayerPrefs based on the mixer identifier.
    private string MusicVolumeKey { get { return mixerIdentifier + "MusicVolume"; } }
    private string SFXVolumeKey { get { return mixerIdentifier + "SFXVolume"; } }

    private void Start()
    {
        // Check if the unique music volume key exists for this scene
        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            LoadVolume();
        }
        else
        {
            // Set default slider values (0.5f is a common mid-level default)
            musicSlider.value = 0.5f;
            SFXSlider.value = 0.5f;
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        // Use clamp to ensure the value is never 0 because Mathf.Log10(0) is undefined.
        float volume = Mathf.Clamp(musicSlider.value, 0.0001f, 1f);

        // Convert to decibels (dB). Multiplying by 20 is common when converting a linear slider value to dB.
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);

        // Save the volume setting using the unique key.
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Clamp(SFXSlider.value, 0.0001f, 1f);
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        // Load saved values using the unique keys.
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        SFXSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);

        // Update the AudioMixer with the loaded values.
        SetMusicVolume();
        SetSFXVolume();
    }
}
