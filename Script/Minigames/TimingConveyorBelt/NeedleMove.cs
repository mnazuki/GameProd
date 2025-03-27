using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class NeedleMove : MonoBehaviour
{
    [SerializeField] GameObject leftPosTar, rigthPosTar;
    public float Speed;
    private float leftPosX, leftPosY, rightPos;
    private bool isInsideBar;
    public bool isScoreOnce, tookDamage, scoredOrMissed;

    private PlayerHealth playerHealthSC;
    private MoleculeConveyor moleculeConveyor;

    private void Start()
    {
        playerHealthSC = FindFirstObjectByType<PlayerHealth>();
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();

        //initialize pin positioning
        leftPosY = leftPosTar.transform.position.y;
        leftPosX = leftPosTar.transform.position.x;
        rightPos = rigthPosTar.transform.position.x;

        scoredOrMissed = false;
        isScoreOnce = false;
        tookDamage = false;

        this.transform.position = new Vector2 (leftPosX, leftPosY);
        this.transform.DOMoveX(rightPos, Speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (isInsideBar && playerHealthSC.health >= 1 && !scoredOrMissed && moleculeConveyor.isCenter)
            {
                    playerHealthSC.score++;
                    isScoreOnce = true;
                Debug.Log("Timed");
            }

            if (isInsideBar == false && playerHealthSC.health > 0 && !scoredOrMissed && moleculeConveyor.isCenter)
            {
                    tookDamage = true;
                    playerHealthSC.health--;
                    playerHealthSC.UpdateHeartsUI();
                    Debug.Log("Missed");
            }

            if (isInsideBar == false && playerHealthSC.health == 0)
            {
                Debug.Log("Missed & ran out of health, 'stopping' game");
                playerHealthSC.UpdateHeartsUI();
            }
        }

        if (isScoreOnce || tookDamage)
        {
            scoredOrMissed = true;
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
