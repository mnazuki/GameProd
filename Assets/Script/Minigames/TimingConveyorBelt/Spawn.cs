using DG.Tweening;
using System.Threading;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    /*[SerializeField] GameObject molecule, bar;//add molecule types depending on hardmode*/
    [SerializeField] Transform initialTarget, rightPos, leftPos;
    [SerializeField] Transform Canvas;
    [SerializeField] GameObject[] molecule;
    [SerializeField] GameObject bar;

    private MoleculeConveyor moleculeConveyor;
    private NeedleMove needleMove;

    private GameObject newMolecule, newBar;
    private float timerSpawn, randomBarPosX, rightPosX, barY, leftPosX;
    private int randomSpawn;


    void Start()
    {
        rightPosX = rightPos.position.x;
        leftPosX = leftPos.position.x;
        barY = rightPos.position.y;

        needleMove = FindFirstObjectByType<NeedleMove>();
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();



        randomBarPosX = Random.Range(340f, 1545f);
        newMolecule = Instantiate(molecule[0], initialTarget);
        newBar = Instantiate(bar, initialTarget);
        NormalMolecule();


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
            randomSpawn = Random.Range(0, molecule.Length);
            randomBarPosX = Random.Range(leftPosX, rightPosX);

            newMolecule = Instantiate(molecule[randomSpawn], initialTarget);
            newBar = Instantiate(bar, initialTarget);

            timerSpawn = 0;
            moleculeConveyor.toDestroy = false;
        }

        NormalMolecule();
        MediumMolecule();
        HardMolecule();
    }

    public void NormalMolecule()
    {
        if (randomSpawn == 0 && newBar != null) 
        {
            newBar.transform.position = new Vector2(randomBarPosX,barY);
            needleMove.needleSpeed = 12f;
        }
    }

    public void MediumMolecule()
    {
        if (randomSpawn == 1 && newBar != null)
        {
            newBar.transform.position = new Vector2(randomBarPosX, barY);
            newBar.transform.localScale = new Vector2(1f,1f);
            needleMove.needleSpeed = 20f;
        }
    }

    public void HardMolecule()
    {
        if (randomSpawn == 2 && newBar != null)
        {
            newBar.transform.position = new Vector2(randomBarPosX, barY);
            newBar.transform.localScale = new Vector2(1f, 1f);
            needleMove.needleSpeed = 12f;

            float pingPongValue = Mathf.PingPong(Time.time * 10 * 100, rightPosX - leftPosX);
            newBar.transform.position = new Vector2(rightPosX - pingPongValue, barY);

        }
    }
}
