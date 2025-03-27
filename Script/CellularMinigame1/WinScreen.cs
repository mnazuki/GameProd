using UnityEngine;
using UnityEngine.UI    ;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
     //[NEW] Activates WinScreen Canvas
    public void win(){
        gameObject.SetActive(true);
    }

    //Button for next level
    public void Proceed(){
       SceneManager.LoadScene("ElectronTransportChain");
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
