using UnityEngine;
using UnityEngine.UI    ;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
     
    [Header("Scene Settings")]
    [SerializeField] private string nextLevelScene;
    [SerializeField] private string restartScene;
    [SerializeField] private string mainMenu;

    public void win(){
        gameObject.SetActive(true);
    }

    //Button for next level
    public void Proceed(){
       SceneManager.LoadScene(nextLevelScene);
    }

    public void Restart(){
         SceneManager.LoadScene(restartScene);
    }

    public void Quit(){
        SceneManager.LoadScene(mainMenu);
//         #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #else
//         Application.Quit();
// #endif
    }
}
