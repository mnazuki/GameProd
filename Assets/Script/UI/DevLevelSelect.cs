using UnityEngine;
using UnityEngine.UI    ;
using UnityEngine.SceneManagement;

public class DevLevelSelect : MonoBehaviour
{
     
    [Header("Scene Settings")]
    [SerializeField] private string Glycolysis;
    [SerializeField] private string ETC;
    [SerializeField] private string Anabolism;
    [SerializeField] private string Catabolism;
    [SerializeField] private string Amphibolism;
    

    public void win(){
        gameObject.SetActive(true);
    }

    //Button for next level
    public void LoadGlycolysis(){
       SceneManager.LoadScene(Glycolysis);
    } public void LoadETC(){
       SceneManager.LoadScene(ETC);
    } public void LoadAnabolism(){
       SceneManager.LoadScene(Anabolism);
    } public void LoadCatabolism(){
       SceneManager.LoadScene(Catabolism);
    } public void LoadAmphibolism(){
       SceneManager.LoadScene(Amphibolism);
    }

    public void Quit(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
