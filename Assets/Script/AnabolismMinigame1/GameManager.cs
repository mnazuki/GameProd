using UnityEngine;

public class AnabolismGameManager : MonoBehaviour
{
    public GameObject hintScreen;
    public bool hintScreenOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (hintScreenOpen)
            {
                hintScreenOpen = false;
                hintScreen.SetActive(false);
                Debug.Log("Tab pressed. Hint screen closed.");
                Debug.Log("Resuming, timescale = 1");
                Time.timeScale = 1f;
            }
            else
            {
                hintScreenOpen = true;
                hintScreen.SetActive(true);
                Debug.Log("Tab pressed. Hint screen opened.");
                Debug.Log("Pausing, timescale = 0");
                Time.timeScale = 0f;
            }
        }
    }
}