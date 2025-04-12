using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject d1;

    void Start(){
         StartCoroutine(DelayedSetActive());
    }

    void Update(){
        if (d1 == null){
            goToLoading();
        }
    }
  
    private IEnumerator DelayedSetActive()
    {
        yield return new WaitForSeconds(4f);
        dialogue.SetActive(true);
    }

    public void goToLoading(){
        SceneManager.LoadScene("Loading Scene");
    }
}