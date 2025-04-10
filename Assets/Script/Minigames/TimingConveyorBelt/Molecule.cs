using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Molecule : MonoBehaviour
{
    [SerializeField] Sprite successMolecule;
    [SerializeField] Sprite missedMolecule;
    private MoleculeConveyor moleculeConveyor;
    private NeedleMove needleMove;
    public GameObject moleculeObj;
    private Image thisSprite;
    private bool flag1, flag2;
    private void Start()
    {
        thisSprite = this.GetComponent<Image>();
        flag1 = true;
        flag2 = true;
        moleculeObj = this.gameObject;
        moleculeConveyor = FindFirstObjectByType<MoleculeConveyor>();
        needleMove = FindFirstObjectByType<NeedleMove>();
        this.transform.position = moleculeConveyor.initialPos;
    }

    private void FixedUpdate()
    {
        moleculeConveyor.MoleculeMove();
    }

    private void Update()
    {
        if (needleMove.tookDamage)
        {
            if (flag1)
            {
                thisSprite.sprite = missedMolecule;
                thisSprite.SetNativeSize();
                flag1 = false;
            }

        }

        if (needleMove.isScoreOnce)
        {
            if (flag2)
            {
                thisSprite.sprite = successMolecule;
                thisSprite.SetNativeSize();
                flag2 = false;
            }
        }


        if (moleculeObj == null)
        {
            Debug.Log("this Obj is null");
        }

        if(moleculeConveyor.toDestroy)
        {
            flag1 = true;
            flag2 = true;
            Destroy(this.gameObject);
        }
    }
}
