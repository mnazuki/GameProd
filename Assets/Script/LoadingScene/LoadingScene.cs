using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private float loadDelay = 4f; // Time to wait before loading the next scene
    [SerializeField] private string nextSceneName; // The name of the next scene to load

    private void Start()
    {
        Time.timeScale = 1f;
        // Start the coroutine to load the scene after a delay
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private System.Collections.IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(loadDelay);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
