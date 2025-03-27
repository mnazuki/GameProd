using Unity.VisualScripting;
using UnityEngine;

public class Molecule : MonoBehaviour
{
    private MoleculeConveyor moleculeConveyor;
    public GameObject moleculeObj;
    private void Start()
    {
        moleculeObj = this.gameObject;
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();
        this.transform.position = moleculeConveyor.initialPos;
    }

    void Update()
    {
        moleculeConveyor.MoleculeMove();

        if (moleculeObj == null)
        {
            Debug.Log("this Obj is null");
        }

        if(moleculeConveyor.toDestroy)
        {
            Destroy(this.gameObject);
        }
    }
}
