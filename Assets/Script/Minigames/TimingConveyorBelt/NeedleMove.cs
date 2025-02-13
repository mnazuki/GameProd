using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class NeedleMove : MonoBehaviour
{
    [SerializeField] GameObject leftPosTar, rigthPosTar;
    public float Speed;
    private float leftPosX, leftPosY, rightPos;
    private bool isInsideBar;

    private PlayerHealth playerHealthSC;

    private void Start()
    {
        playerHealthSC = FindObjectOfType<PlayerHealth>();

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
            if (isInsideBar && playerHealthSC.health >= 1)
            {
                Debug.Log("Timed");
            }

            if (isInsideBar == false && playerHealthSC.health > 0)
            {
                Debug.Log("Missed");
                playerHealthSC.health--;
                playerHealthSC.UpdateHeartsUI();
            }

            if (isInsideBar == false && playerHealthSC.health == 0)
            {
                Debug.Log("Missed & ran out of health, 'stopping' game");
                playerHealthSC.UpdateHeartsUI();
                Time.timeScale = 0; //this just "turns the game off" or turns time to 0
                //nilagay ko lang to parang fake game over
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
