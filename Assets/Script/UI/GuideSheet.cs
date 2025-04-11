using UnityEngine;

public class GuideSheet : MonoBehaviour
{

    [SerializeField] private GameObject guideSheet;
    private bool guideToggle = false;

    [SerializeField] private AudioSource[] src;

    // Update is called once per frame
    void Start(){
        Time.timeScale = 1f; // To prevent bugs
        src = GameObject.Find("SFX")?.GetComponents<AudioSource>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)){
            
           if (!guideToggle){
            guideOn();
           }else{
            guideOff();
           }
        }
    }

    private void guideOn(){

         if (guideSheet != null){
                guideSheet.SetActive(true);

                Time.timeScale = 0f;
                guideToggle = true;
                src[1].Play();
            }
    }

    private void guideOff(){
        if (guideSheet != null){
                guideSheet.SetActive(false);

                Time.timeScale = 1f;
                guideToggle = false;
                src[1].Play();
            }

    }
}
