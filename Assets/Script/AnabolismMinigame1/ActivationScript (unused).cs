using UnityEngine;
using System.Collections;
using NUnit.Framework.Internal;

public class ActivationScript : MonoBehaviour
{
    public GameObject glucosePrefab;
    public GameObject glucose6PhosphatePrefab;

    public GameObject atpPrefab;

    private GameObject glucoseInstance;
    private GameObject atpInstance;
    private GameObject newMoleculeInstance;

    [SerializeField] private bool isGlucoseIn = false;
    [SerializeField] private bool isATPIn = false;
    [SerializeField] private int glucose6Made = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Glucose"))
        {
            //second glucose inside
            if (isGlucoseIn)
            {
                Debug.Log("Invalid combination! Two Glucose molecules detected. Destroying...");
                isGlucoseIn = false;
                Destroy(collision.gameObject);
                Destroy(glucoseInstance);
                
                return;
            }
            glucoseInstance = collision.gameObject;
            isGlucoseIn = true;
            Debug.Log("glucose inside");
        }
        if (collision.gameObject.name.Contains("ATP"))
        {
            //second atp inside
            if (isATPIn)
            {
                Debug.Log("Invalid combination! Two ATP molecules detected. Destroying...");
                isATPIn = false;
                Destroy(collision.gameObject);
                Destroy(atpInstance);
                
                return;
            }
            atpInstance = collision.gameObject;
            isATPIn = true;
            Debug.Log("atp inside");
        }
        if (isGlucoseIn == true && isATPIn == true)
        {
            Destroy(glucoseInstance);
            Destroy(atpInstance);
            newMoleculeInstance = Instantiate(glucose6PhosphatePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            Debug.Log("Spawning activated glucose: glucose 6-chuchuchu");
            isATPIn = false;
            isGlucoseIn = false;
            glucose6Made++;
        }
    }
}