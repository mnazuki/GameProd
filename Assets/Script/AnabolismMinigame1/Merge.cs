using UnityEngine;

public class Merge : MonoBehaviour
{
    //not being used
    //public GameObject Fructose;
    //public GameObject Galactose;
    //public GameObject Glucose;

    //public GameObject ATP;

    //private TaskManager taskManager;
    //private MoleculeSpawner moleculeSpawner;

    //private bool glucoseInActivationZone = false;
    //private bool atpInActivationZone = false;

    //private void Start()
    //{
    //    taskManager = FindAnyObjectByType<TaskManager>();
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Activation"))
    //    {
    //        if (gameObject.name.Contains("Glucose")) 
    //        {
    //            glucoseInActivationZone = true;
    //            Debug.Log("Glucose is in activation area");
    //        }
    //        if (gameObject.name.Contains("ATP"))
    //        {
    //            atpInActivationZone = true;
    //            Debug.Log("ATP is in activation area");
    //        }
    //        if ((glucoseInActivationZone == true) && (atpInActivationZone == true))
    //        {
    //            Debug.Log("Glucose & ATP in activation area");
    //        }
            //if (gameObject.name.Contains("Fructose"))
            //{
            //    Debug.Log("Fructose is in activation area");
            //}
            //if (gameObject.name.Contains("Galactose"))
            //{
            //    Debug.Log("Galactose is in activation area");
            //}

    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Contains("Glucose"))
    //    {
    //        Glucose = null;
    //        Debug.Log("Glucose left activation area");
    //    }
    //    if (collision.gameObject.name.Contains("ATP"))
    //    {
    //        ATP = null;
    //        Debug.Log("ATP left activation area");
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision) 
    //{
    //    SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
    //    SpriteRenderer otherRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
    //    moleculeSpawner = FindAnyObjectByType<MoleculeSpawner>();

    //    if (moleculeSpawner.isGameFinished == false)
    //    {
    //        if (mainRenderer != null && otherRenderer != null)
    //        {
    //            Sprite mainSprite = mainRenderer.sprite;
    //            Sprite otherSprite = otherRenderer.sprite;

    //            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;





                //if ((mainSprite.name.Contains("red") && otherSprite.name.Contains("blu")))
                //{
                //    Debug.Log("Spawning Purple Molecule!");
                //    GameObject newMolecule = Instantiate(purpleMolecule, spawnPosition, Quaternion.identity);
                //    newMolecule.SetActive(true);

                //    if (taskManager != null) taskManager.MoleculeCreated("purple");

                //    Destroy(collision.gameObject);
                //    Destroy(gameObject);

                //}
                //else if ((mainSprite.name.Contains("red") && otherSprite.name.Contains("yellow")))
                //{
                //    Debug.Log("Spawning Orange Molecule!");
                //    GameObject newMolecule = Instantiate(orangeMolecule, spawnPosition, Quaternion.identity);
                //    newMolecule.SetActive(true);

                //    if (taskManager != null) taskManager.MoleculeCreated("orange");

                //    Destroy(collision.gameObject);
                //    Destroy(gameObject);

                //}
                //else if ((mainSprite.name.Contains("blu") && otherSprite.name.Contains("yellow")))
                //{
                //    Debug.Log("Spawning Green Molecule!");
                //    GameObject newMolecule = Instantiate(greenMolecule, spawnPosition, Quaternion.identity);
                //    newMolecule.SetActive(true);

                //    if (taskManager != null) taskManager.MoleculeCreated("green");

                //    Destroy(collision.gameObject);
                //    Destroy(gameObject);

                //}
            //}
    //    }

    //}
}
