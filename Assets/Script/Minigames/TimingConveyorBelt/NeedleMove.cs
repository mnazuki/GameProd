using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class NeedleMove : MonoBehaviour
{
    [SerializeField] GameObject leftPosTar, rigthPosTar;
    public float Speed;
    private float leftPosX, leftPosY, rightPos;
    private bool isInsideBar;

    private void Start()
    {
        //initialize pin positioning
        leftPosY = leftPosTar.transform.position.y;
        leftPosX = leftPosTar.transform.position.x;
        rightPos = rigthPosTar.transform.position.x;

        this.transform.position = new Vector2 (leftPosX, leftPosY);
        this.transform.DOMoveX(rightPos, Speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (isInsideBar)
            {
                Debug.Log("Timed");
            }
            else
            {
                Debug.Log("Missed");
                //DITO YUNG LIFE SYSTEM
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HighlightedBar"))
        {
            isInsideBar = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HighlightedBar"))
        {
            isInsideBar = false;
        }
    }
}
