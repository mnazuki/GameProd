using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoleculeConveyor : MonoBehaviour
{
    [SerializeField] Transform initialTarget, middleTarget, finalTarget;
    [SerializeField] Slider timerSlider;
    [SerializeField] GameObject spacebarUI;
    public float barSpeed/*To be Used*/, maxTimer, conveyorSpeed, timer; //change this depending on difficulty
    public Vector3 middlePos, finalPos;
    public Vector3 initialPos;
    public bool isCenter, toDestroy;
    private GameObject moleculeToMove;


    private PlayerHealth playerHealthSC;
    private NeedleMove needleMove;

    private void Start()
    {
        timerSlider.maxValue = maxTimer;
        playerHealthSC = FindFirstObjectByType<PlayerHealth>();
        needleMove = FindFirstObjectByType<NeedleMove>();



        isCenter = false;
        toDestroy = false;

        initialTarget = GameObject.FindGameObjectWithTag("InitialTarget")?.transform;
        middleTarget = GameObject.FindGameObjectWithTag("MiddleTarget")?.transform;
        finalTarget = GameObject.FindGameObjectWithTag("FinalTarget")?.transform;
        

        initialPos = initialTarget.position;
        middlePos = middleTarget.position;
        finalPos = finalTarget.position;

    }

    private void Update()
    {

        timerSlider.value = maxTimer - timer;

        moleculeToMove = GameObject.FindGameObjectWithTag("Molecule");
    }

    public void MoleculeMove()
    {

        if (moleculeToMove != null)
        {
            if (!isCenter && !needleMove.scoredOrMissed)
            {
                MoveToCenter();
            }

            if (moleculeToMove.transform.position == middlePos)
            {
                isCenter = true;
            }

            if (isCenter)
            {
                spacebarUI.SetActive(true);
                timer += Time.deltaTime;
                timer = Mathf.Clamp(timer, 0, maxTimer);
            }

            if (needleMove.scoredOrMissed && timer < maxTimer)
            {
                spacebarUI.SetActive(false);
                Debug.Log("Hit or Missed");
                MoveToFinal();
                isCenter = false;
            }

            if (timer >= maxTimer)
            {
                Debug.Log("TooSlow");
                TakeDamageOnce();
                MoveToFinal();
                isCenter = false;

            }

            if (!isCenter && moleculeToMove.transform.position == finalPos)
            {
                timer = 0;
                needleMove.tookDamage = false;
                needleMove.isScoreOnce = false;
                needleMove.scoredOrMissed = false;
                toDestroy = true;

                Debug.Log("Final Pos Done, should spawn");
            }
        }
       
    }

    public void MoveToCenter()
    {
        moleculeToMove.transform.position = Vector2.MoveTowards(moleculeToMove.transform.position, middlePos, conveyorSpeed);
    }
    public void MoveToFinal()
    {
        moleculeToMove.transform.position = Vector2.MoveTowards(moleculeToMove.transform.position, finalPos, conveyorSpeed);
    }

    public void TakeDamageOnce()
    {
        if (!needleMove.tookDamage)
        {
            playerHealthSC.health--;
            playerHealthSC.UpdateHeartsUI();
            needleMove.tookDamage = true;
        }
    }
}
