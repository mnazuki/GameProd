using UnityEngine;
using UnityEngine.UI    ;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    //Activates GameOver Canvas

    [Header("Scene Settings")]
    [SerializeField] private string restartScene;
    [SerializeField] private string mainMenu;
    public void gmOver(){
        gameObject.SetActive(true);
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
