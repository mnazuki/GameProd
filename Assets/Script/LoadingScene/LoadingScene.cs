using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Delay before loading the one-time scene")]
    [SerializeField] private float loadDelay = 4f;

    [Tooltip("The name of the one-time scene that should run only once (until PlayerPrefs are reset)")]
    [SerializeField] private string oneTimeSceneName;

    [Tooltip("The name of the fallback scene to load if the one-time scene has already been shown")]
    [SerializeField] private string fallbackSceneName;

    [Header("PlayerPrefs Settings")]
    [Tooltip("The key used in PlayerPrefs to mark if the one-time scene has been shown")]
    [SerializeField] private string playerPrefKey = "LoadingSceneShown";

    private void Start()
    {
        Time.timeScale = 1f;

        // Check if the one-time scene has been shown.
        if (PlayerPrefs.GetInt(playerPrefKey, 0) == 0)
        {
            // Not shown yet: wait for the delay and then load the one-time scene.
            StartCoroutine(LoadOneTimeScene());

            // Set the flag so that on future loads the one-time scene is skipped.
            PlayerPrefs.SetInt(playerPrefKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Already shown: immediately load the fallback scene.
            SceneManager.LoadScene(fallbackSceneName);
        }
    }

    private IEnumerator LoadOneTimeScene()
    {
        // Wait for the specified delay before loading the scene.
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(oneTimeSceneName);
    }
}
