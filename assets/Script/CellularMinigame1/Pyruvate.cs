// placed on the pyruvate 1 & 2 prefabs (add component)

using UnityEngine;

public class Pyruvate : MonoBehaviour
{
    private Vector3 targetPosition; // target position 
    private float moveSpeed = 0.7f; // speed 

    void Start()
    {
        SetTargetPosition();
    }

    void Update()
    {
        MovePyruvate();
    }

    public void SetTargetPosition(){
        // set target position 
        targetPosition = transform.position + new Vector3(8f, 0, 0); // moves 8 units to the right
    }

    public void MovePyruvate(){
        // move the pyruvate using Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    // if pyruvates hits the game objecta tagged "FunnelCollider", 
    // pyruvates will be destroyed
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FunnelCollider"))
        {
            // Debug.Log("Pyruvate hit FunnelCollider!");
            Destroy(gameObject);

            // call the spawner to prevent multiple instances from forming if not null, proceed
            if (PyruvateSpawner.Instance != null)
            {   
                // check if all ATP and NADH are collected before spawning mini pyruvates
                if (PyruvateSpawner.Instance.AreAllMoleculesCollected())
                {
                    // if yes, spawn mini pyruvates
                    PyruvateSpawner.Instance.SpawnMiniPyruvates();
                }
                // else
                // {
                //     // Debug.Log("Mini pyruvates will NOT spawn. Collect all ATP and NADH first.");
                // }
            }
            
        }
    }
}