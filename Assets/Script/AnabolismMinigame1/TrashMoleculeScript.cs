using UnityEngine;

public class TrashMoleculeScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Molecule"))
        {
            Debug.Log($"Destroyed {collision.gameObject.name} in trash area");
            Destroy(collision.gameObject);
        }
    }
}
