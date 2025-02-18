using UnityEngine;

public class CollectionScript : MonoBehaviour
{
    [SerializeField] private int sucroseCollected;
    [SerializeField] private int lactoseCollected;
    [SerializeField] private int maltoseCollected;
    [SerializeField] private int dnaCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CombinationScript.moleculeInstances.Contains(collision.gameObject))
        {
            // This object is in the nucleotideInstances list, so we collect it
            Debug.Log("Collected: " + collision.gameObject.name);
            // Increment the appropriate counter based on the name of the object
            if (collision.gameObject.name.Contains("Sucrose"))
            {
                sucroseCollected++;
                Debug.Log("Sucrose collected. Total: " + sucroseCollected);
            }
            else if (collision.gameObject.name.Contains("Lactose"))
            {
                lactoseCollected++;
                Debug.Log("Lactose collected. Total: " + lactoseCollected);
            }
            else if (collision.gameObject.name.Contains("Maltose"))
            {
                maltoseCollected++;
                Debug.Log("Maltose collected. Total: " + maltoseCollected);
            }
            else if (collision.gameObject.name.Contains("DNA"))
            {
                dnaCollected++;
                Debug.Log("DNA collected. Total: " + dnaCollected);
            }
            CombinationScript.moleculeInstances.Remove(collision.gameObject); // Remove it from the collection list
            Destroy(collision.gameObject); // Destroy the collected instance
            
        }
        else
        {
            Destroy(collision.gameObject);
            Debug.Log("Not a valid object to collect.");
        }
    }
}
