using System;
using Unity.VisualScripting;
using UnityEngine;

public class MoleculeConveyor : MonoBehaviour
{
    [SerializeField] Transform initialTarget, middleTarget, finalTarget;
    public float barSpeed/*To be Used*/, maxTimer, conveyorSpeed; //change this depending on difficulty
    public Vector3 middlePos, finalPos;
    public Vector3 initialPos;
    public bool isCenter, toDestroy;
    public float timer;
    private GameObject moleculeToMove;


    private PlayerHealth playerHealthSC;
    private NeedleMove needleMove;

    private void Start()
    {
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
        moleculeToMove = GameObject.FindGameObjectWithTag("Molecule");
    }

    public void MoleculeMove()
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
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, maxTimer);
        }

        if (needleMove.scoredOrMissed && timer < maxTimer)
        {
            Debug.Log("Hit or Missed");
            MoveToFinal();
            isCenter = false;
        }

        if (timer >= 3)
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
