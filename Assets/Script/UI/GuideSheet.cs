using UnityEngine;

public class GuideSheet : MonoBehaviour
{

    [SerializeField] private GameObject guideSheet;
    private bool guideToggle = false;

    // Update is called once per frame
    void Start(){
        Time.timeScale = 1f; // To prevent bugs
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
            }
    }

    private void guideOff(){
        if (guideSheet != null){
                guideSheet.SetActive(false);

                Time.timeScale = 1f;
                guideToggle = false;
            }

    }
}
