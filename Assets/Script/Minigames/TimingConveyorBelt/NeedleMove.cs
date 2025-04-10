using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class NeedleMove : MonoBehaviour
{
    [SerializeField] Transform leftPosTar, rigthPosTar;
    public float needleSpeed;
    private float leftPosX, leftPosY, rightPos;
    private bool isInsideBar;
    public bool isScoreOnce, tookDamage, scoredOrMissed;


    private PlayerHealth playerHealthSC;
    private MoleculeConveyor moleculeConveyor;

    private void Start()
    {
        playerHealthSC = FindFirstObjectByType<PlayerHealth>();
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();

        //initialize pin positioning // maybe no need to use tweening para nasa update nalang sya,, aka making it scalable
        leftPosY = leftPosTar.position.y;
        leftPosX = leftPosTar.position.x;
        rightPos = rigthPosTar.position.x;

        scoredOrMissed = false;
        isScoreOnce = false;
        tookDamage = false;

    }

    private void FixedUpdate()
    {
        float pingPongValue = Mathf.PingPong(Time.time * needleSpeed * 100, rightPos - leftPosX);
        this.transform.position = new Vector2(leftPosX + pingPongValue, transform.position.y);
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
