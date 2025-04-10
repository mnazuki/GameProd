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
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider2D>(); //get the collider component for disabling/enabling

        if (objectCollider == null)
        {
            Debug.LogError("Collider not found on " + gameObject.name);
        }
    }

    void OnMouseDown()
    {
        if (moleculeSpawner.isGameFinished)
            return;

        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }

        offset = transform.position - GetMouseWorldPos();
    
    }

    void OnMouseDrag()
    {
        if (moleculeSpawner.isGameFinished)
            return;
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
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void OnDestroy()
    {
        if (MoleculeSpawner.Instance != null)
        {
            MoleculeSpawner.Instance.DecreaseMoleculeCount();
        }
    }
}
