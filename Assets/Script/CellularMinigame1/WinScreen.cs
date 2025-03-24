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
         SceneManager.LoadScene("CellularMinigame1");
    }
}
