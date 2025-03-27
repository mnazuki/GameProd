using UnityEngine;
using UnityEngine.UI    ;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    //Activates GameOver Canvas
    public void gmOver(){
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("Glycolysis");
    }

    public void Quit(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
