using System;
using Unity.VisualScripting;
using UnityEngine;

public class MoleculeConveyor : MonoBehaviour
{
    [SerializeField] Transform initialTarget, middleTarget, finalTarget;
    public Sprite missedMolecule;
    public float barSpeed, conveyorSpeed, maxTimer; //change this depending on difficulty
    private Vector3 initialPos, middlePos, finalPos;
    private bool tookDamage, isCenter;
    private float timer;

    private PlayerHealth playerHealthSC;

    private void Start()
    {
        playerHealthSC = FindObjectOfType<PlayerHealth>();

        tookDamage = false;
        initialPos = initialTarget.position;
        middlePos = middleTarget.position;
        finalPos = finalTarget.position;

        this.transform.position = initialPos;
    }

    private void Update()
    {
        
        MoveToCenter();

        if (this.transform.position == middlePos && playerHealthSC.health > 0 /*iscenter*/)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, maxTimer);
        }

        if (timer >= 3 && playerHealthSC.health > 0)
        {
            Debug.Log("TooSlow");
            TakeDamageOnce();

            this.transform.position = Vector2.MoveTowards(this.transform.position, finalPos, conveyorSpeed); //need Iscenter handler
            MoveToFinal();
            if (this.transform.position == finalPos)
            {
                tookDamage = false;
                
                Destroy(this.gameObject);
            }

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
        if (!tookDamage)
        {
            playerHealthSC.health--;
            playerHealthSC.UpdateHeartsUI();
            tookDamage = true;
        }
    }
}
