using System.Threading;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject molecule;
    [SerializeField] Transform initialTarget;
    private MoleculeConveyor moleculeConveyor;
    private float timerSpawn;

    void Start()
    {
        GameObject newMolecule = Instantiate(molecule, initialTarget);
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Destroy" + moleculeConveyor.toDestroy);

        if (moleculeConveyor.toDestroy) 
        {
            timerSpawn += Time.deltaTime;
        }


        if (moleculeConveyor.toDestroy && timerSpawn >= 1)
        {
            timerSpawn = 0;
            moleculeConveyor.toDestroy = false;
            Debug.Log("Molecule spawn");
            GameObject newMolecule = Instantiate(molecule, initialTarget);
            newMolecule.SetActive(true);
        }
    }
}
