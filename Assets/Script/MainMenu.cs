using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("AudioSource to play the button click sound")]
    public AudioSource buttonAudioSource;

    [Tooltip("AudioClip to play when the Play button is clicked")]
    public AudioClip playButtonClickSound;

    [Tooltip("AudioClip to play when the Quit button is clicked")]
    public AudioClip quitButtonClickSound;

    /// <summary>
    /// Called when the Play button is clicked.
    /// Plays the audio clip, waits for it to finish, then loads the next scene.
    /// </summary>
    public void Play()
    {
        // Loads the next scene (current scene index + 1)
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(PlaySoundAndLoadScene(nextSceneIndex, playButtonClickSound));
    }

    /// <summary>
    /// Called when the Quit button is clicked.
    /// Plays the audio clip, waits for it to finish, then quits the application.
    /// </summary>
    public void Quit()
    {
        StartCoroutine(PlaySoundAndQuit(quitButtonClickSound));
    }

    /// <summary>
    /// Coroutine that plays the given audio clip and loads the scene after the clip finishes.
    /// </summary>
    /// <param name="sceneIndex">The build index of the scene to load.</param>
    /// <param name="clip">The audio clip to play.</param>
    /// <returns></returns>
    private IEnumerator PlaySoundAndLoadScene(int sceneIndex, AudioClip clip)
    {
        if (buttonAudioSource != null && clip != null)
        {
            buttonAudioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Coroutine that plays the given audio clip and quits the application after the clip finishes.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <returns></returns>
    private IEnumerator PlaySoundAndQuit(AudioClip clip)
    {
        if (buttonAudioSource != null && clip != null)
        {
            buttonAudioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        Application.Quit();
        Debug.Log("Player Has Quit");
    }
}
