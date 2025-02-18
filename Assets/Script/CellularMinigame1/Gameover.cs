using UnityEngine;
using UnityEngine.UI    ;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    //[NEW] Activates GameOver Canvas
    public void gmOver(){
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("CellularMinigame1");
    }
}
