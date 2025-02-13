using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private Collider2D objectCollider;

    private MoleculeSpawner moleculeSpawner;

    void Start()
    {
        moleculeSpawner = FindAnyObjectByType<MoleculeSpawner>();
        mainCamera = Camera.main;  // Store the reference to the main camera
        objectCollider = GetComponent<Collider2D>();  // Get the collider component for disabling/enabling

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found! Make sure there is a camera tagged 'MainCamera'.");
        }

        if (objectCollider == null)
        {
            Debug.LogError("Collider not found on " + gameObject.name);
        }
    }

    void OnMouseDown()
    {
        if (moleculeSpawner.isGameFinished)
            return;

        // Disable collider to prevent merging during drag
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }

        // Calculate the offset between the object and the mouse position
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (moleculeSpawner.isGameFinished)
            return;
        // Move the object with the mouse while dragging
        if (mainCamera != null)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
        else
        {
            Debug.LogError("Main camera reference is missing!");
        }
    }

    void OnMouseUp()
    {
        if (moleculeSpawner.isGameFinished)
            return;
        // Re-enable the collider after dragging
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convert mouse position from screen space to world space
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;  // Set correct Z distance
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
