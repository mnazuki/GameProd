using UnityEngine;

public class Hitbar : MonoBehaviour
{
    private MoleculeConveyor moleculeConveyor;
    void Start()
    {
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();
    }

    void Update()
    {
        if (moleculeConveyor.toDestroy)
        {
            Destroy(this.gameObject);
        }
    }
}
