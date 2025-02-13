using UnityEngine;

public class Merge : MonoBehaviour
{
    public GameObject redMaterial;
    public GameObject blueMaterial;
    public GameObject yellowMaterial;
    public GameObject purpleMaterial;
    public GameObject orangeMaterial;
    public GameObject greenMaterial;

    public GameObject purpleMolecule;
    public GameObject orangeMolecule;
    public GameObject greenMolecule;

    private TaskManager taskManager;
    private MoleculeSpawner moleculeSpawner;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer otherRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        moleculeSpawner = FindAnyObjectByType<MoleculeSpawner>();

        if (moleculeSpawner.isGameFinished == false)
        {
            if (mainRenderer != null && otherRenderer != null)
            {
                Sprite mainSprite = mainRenderer.sprite;
                Sprite otherSprite = otherRenderer.sprite;

                Debug.Log($"Collision detected: {mainSprite.name} with {otherSprite.name}");

                Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

                if ((mainSprite.name.Contains("red") && otherSprite.name.Contains("blu")))
                {
                    Debug.Log("Spawning Purple Molecule!");
                    GameObject newMolecule = Instantiate(purpleMolecule, spawnPosition, Quaternion.identity);
                    newMolecule.SetActive(true);

                    if (taskManager != null) taskManager.MoleculeCreated("purple");

                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                }
                else if ((mainSprite.name.Contains("red") && otherSprite.name.Contains("yellow")))
                {
                    Debug.Log("Spawning Orange Molecule!");
                    GameObject newMolecule = Instantiate(orangeMolecule, spawnPosition, Quaternion.identity);
                    newMolecule.SetActive(true);

                    if (taskManager != null) taskManager.MoleculeCreated("orange");

                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                }
                else if ((mainSprite.name.Contains("blu") && otherSprite.name.Contains("yellow")))
                {
                    Debug.Log("Spawning Green Molecule!");
                    GameObject newMolecule = Instantiate(greenMolecule, spawnPosition, Quaternion.identity);
                    newMolecule.SetActive(true);

                    if (taskManager != null) taskManager.MoleculeCreated("green");

                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                }
            }
        }

    }
}