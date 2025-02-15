using System;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MoleculeConveyor : MonoBehaviour
{
    [SerializeField] Transform initialTarget, middleTarget, finalTarget;
    public Sprite missedMolecule;
    public float barSpeed, maxTimer; //change this depending on difficulty
    private Vector3 initialPos, middlePos, finalPos;
    private bool isCenter;
    private float timer, conveyorSpeed;

    private PlayerHealth playerHealthSC;
    private NeedleMove needleMove;

    private void Start()
    {
        playerHealthSC = FindFirstObjectByType<PlayerHealth>();
        needleMove = FindFirstObjectByType<NeedleMove>();

        conveyorSpeed = maxTimer/2.5f;
        isCenter = false;
        initialPos = initialTarget.position;
        middlePos = middleTarget.position;
        finalPos = finalTarget.position;

        this.transform.position = initialPos;
    }

    private void Update()
    {
        if (!isCenter && ! needleMove.tookDamage)
        {
            MoveToCenter();
        }

        if (this.transform.position == middlePos)
        {
            isCenter = true;
        }

        if (isCenter)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, maxTimer);
        }

        if (timer >= 3)
        {
            Debug.Log("TooSlow");
            TakeDamageOnce();
            MoveToFinal();
            isCenter = false;

        }

        if (!isCenter && this.transform.position == finalPos)
        {
            needleMove.tookDamage = false;

            Destroy(this.gameObject);
        }

        Debug.Log(timer);
/*      if (isCenter &&)
        {

        }*/
    }

    public void MoveToCenter()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, middlePos, conveyorSpeed);
    }
    public void MoveToFinal()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, finalPos, conveyorSpeed);
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
