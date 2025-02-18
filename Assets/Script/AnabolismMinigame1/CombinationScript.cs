using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombinationScript : MonoBehaviour
{
    public static List<GameObject> moleculeInstances = new List<GameObject>();

    //carbs
    public GameObject glucosePrefab, 
        fructosePrefab, 
        galactosePrefab;

    public GameObject sucrosePrefab, 
        lactosePrefab, 
        maltosePrefab;

    private GameObject glucoseInstance, glucoseInstance2, 
        fructoseInstance, fructoseInstance2, 
        galactoseInstance, galactoseInstance2;

    private GameObject sucroseInstance, 
        lactoseInstance, 
        maltoseInstance;

    [SerializeField]
    private bool isGlucoseIn = false, isSecondGlucoseIn = false, 
        isFructoseIn = false, isSecondFructoseIn = false, 
        isGalactoseIn = false, isSecondGalactoseIn = false;

    [SerializeField] private int sucroseMade = 0, 
        lactoseMade = 0, 
        maltoseMade = 0;

    //nucleotides
    public GameObject adeninePrefab, 
        thyminePrefab, 
        guaninePrefab, 
        cytosinePrefab;

    public GameObject adenineThyminePrefab, 
        guanineCytosinePrefab;

    public GameObject DNAPrefab;

    private GameObject adenineInstance, 
        adenineInstance2, 
        thymineInstance, 
        thymineInstance2,
        guanineInstance, 
        guanineInstance2, 
        cytosineInstance, 
        cytosineInstance2;

    private GameObject adenineThymineInstance, 
        adenineThymineInstance2, 
        guanineCytosineInstance, 
        guanineCytosineInstance2;

    private GameObject DNAInstance;

    [SerializeField]
    private bool isAdenineIn = false, isSecondAdenineIn = false, 
        isThymineIn = false, isSecondThymineIn = false, 
        isGuanineIn = false, isSecondGuanineIn = false, 
        isCytosineIn = false, isSecondCytosineIn = false;

    [SerializeField]
    private bool isAdenineThymineIn = false, isSecondAdenineThymineIn = false,
        isGuanineCytosineIn = false, isSecondGuanineCytosineIn = false;

    [SerializeField] private int adenineThymineMade = 0, 
        guanineCytosineMade = 0, 
        DNAMade = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objName = collision.gameObject.name.Replace("(Clone)", "").Trim();
        //CARBS

        //glucose
        if (objName == ("Glucose"))
        {
            // First glucose
            if (!isGlucoseIn)
            {
                glucoseInstance = collision.gameObject;
                isGlucoseIn = true;
                Debug.Log("glucose 1 in");
            }
            // Second glucose
            else if (!isSecondGlucoseIn)
            {
                glucoseInstance2 = collision.gameObject;
                isSecondGlucoseIn = true;
                Debug.Log("glucose 2 in");
            }
        }

        //fructose
        if (objName == ("Fructose"))
        {
            if (!isFructoseIn)
            {
                Debug.Log("fructose in");
                fructoseInstance = collision.gameObject;
                isFructoseIn = true;
            }
            else if (!isSecondFructoseIn)
            {
                fructoseInstance2 = collision.gameObject;
                isSecondFructoseIn = true;
                Debug.Log("fructose 2 in");
            }
        }

        //galactose
        if (objName == ("Galactose"))
        {
            if (!isGalactoseIn)
            {
                Debug.Log("galactose in");
                galactoseInstance = collision.gameObject; // Corrected here, it should be galactoseInstance, not fructoseInstance
                isGalactoseIn = true;
            }
            else if (!isSecondGalactoseIn)
            {
                galactoseInstance2 = collision.gameObject;
                isSecondGalactoseIn = true;
                Debug.Log("galactose 2 in");
            }
        }

        //NUCLEOTIDES/DNA
        //adenine + thymine base pair
        if (objName == "Adenine+Thymine")
        {
            //first adenine thymine
            if (!isAdenineThymineIn)
            {
                adenineThymineInstance = collision.gameObject;
                isAdenineThymineIn = true;
                Debug.Log("adenine+thymine 1 in");
            }
            //second adenine thymine
            else if (!isSecondAdenineThymineIn)
            {
                adenineThymineInstance2 = collision.gameObject;
                isSecondAdenineThymineIn = true;
                Debug.Log("adenine+thymine 2 in");
            }
        }

        //guanine+cytosine base pair
        else if (objName == "Guanine+Cytosine")
        {
            //first guanine cytosine
            if (!isGuanineCytosineIn)
            {
                guanineCytosineInstance = collision.gameObject;
                isGuanineCytosineIn = true;
                Debug.Log("guanine+cytosine 1 in");
            }
            //second guanine cytosine
            else if (!isSecondGuanineCytosineIn)
            {
                guanineCytosineInstance2 = collision.gameObject;
                isSecondGuanineCytosineIn = true;
                Debug.Log("guanine+cytosine 2 in");
            }
        }

        //adenine
        if (objName == "Adenine")
        {
            // First adenine
            if (!isAdenineIn)
            {
                adenineInstance = collision.gameObject;
                isAdenineIn = true;
                Debug.Log("Adenine 1 in");
            }
            // Second adenine
            else if (!isSecondAdenineIn)
            {
                adenineInstance2 = collision.gameObject;
                isSecondAdenineIn = true;
                Debug.Log("Adenine 2 in");
            }
        }

        //thymine
        if (objName == ("Thymine"))
        {
            // First thymine
            if (!isThymineIn)
            {
                thymineInstance = collision.gameObject;
                isThymineIn = true;
                Debug.Log("Thymine 1 in");
            }
            // Second thymine
            else if (!isSecondThymineIn)
            {
                thymineInstance2 = collision.gameObject;
                isSecondThymineIn = true;
                Debug.Log("Thymine 2 in");
            }
        }

        //guanine
        if (objName == "Guanine")
        {
            // First guanine
            if (!isGuanineIn)
            {
                guanineInstance = collision.gameObject;
                isGuanineIn = true;
                Debug.Log("Guanine 1 in");
            }
            // Second guanine
            else if (!isSecondGuanineIn)
            {
                guanineInstance2 = collision.gameObject;
                isSecondGuanineIn = true;
                Debug.Log("Guanine 2 in");
            }
        }

        //cytosine
        if (objName == "Cytosine")
        {
            // First cytosine
            if (!isCytosineIn)
            {
                cytosineInstance = collision.gameObject;
                isCytosineIn = true;
                Debug.Log("Cytosine 1 in");
            }
            // Second cytosine
            else if (!isSecondCytosineIn)
            {
                cytosineInstance2 = collision.gameObject;
                isSecondCytosineIn = true;
                Debug.Log("Cytosine 2 in");
            }
        }

        CreateDissacharide();
        CreateBasePairs();
        CreateDNA();
    }

    void CreateDissacharide()
    {
        // Valid combinations
        if (isGlucoseIn && isFructoseIn)
        {
            Destroy(glucoseInstance);
            Destroy(fructoseInstance);
            GameObject newSucrose = Instantiate(sucrosePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            moleculeInstances.Add(newSucrose);
            ResetCarbohydratesStates();
            Debug.Log("glucose + fructose = sucrose");
            sucroseMade++;
        }
        else if (isGlucoseIn && isGalactoseIn)
        {
            Destroy(glucoseInstance);
            Destroy(galactoseInstance);
            GameObject newLactose = Instantiate(lactosePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            moleculeInstances.Add(newLactose);
            ResetCarbohydratesStates();
            Debug.Log("glucose + galactose = lactose");
            lactoseMade++;
        }
        else if (isGlucoseIn && isSecondGlucoseIn)
        {
            Destroy(glucoseInstance);
            Destroy(glucoseInstance2);
            GameObject newGlucose = Instantiate(maltosePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            moleculeInstances.Add(newGlucose);
            ResetCarbohydratesStates();
            Debug.Log("glucose + glucose = maltose");
            maltoseMade++;
        }
        else
        {
            //invalid 1
            if (isFructoseIn == true && isGalactoseIn == true)
            {
                Destroy(fructoseInstance);
                Destroy(galactoseInstance);
                ResetCarbohydratesStates();
                Debug.Log("fructose + galactose = nothing, destroying");
            }
            //invalid 2
            else if (isGalactoseIn && isSecondGalactoseIn)
            {
                Destroy(galactoseInstance);
                Destroy(galactoseInstance2);
                ResetCarbohydratesStates();
                Debug.Log("galactose + galactose = nothing, destroying");
            }
            //invalid 3
            else if (isFructoseIn && isSecondFructoseIn)
            {
                Destroy(fructoseInstance);
                Destroy(fructoseInstance2);
                ResetCarbohydratesStates();
                Debug.Log("fructose + fructose = nothing, destroying");
            }
        }

    }
    void ResetCarbohydratesStates()
    {
        isGlucoseIn = false;
        isFructoseIn = false;
        isGalactoseIn = false;
        isSecondGlucoseIn = false;
        isSecondFructoseIn = false;
        isSecondGalactoseIn = false;
    }
    void CreateBasePairs()
    {
        //valid combos
        if (isAdenineIn && isThymineIn) // A + T
        {
            Destroy(adenineInstance);
            Destroy(thymineInstance);
            GameObject newAdenineThymine = Instantiate(adenineThyminePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            moleculeInstances.Add(newAdenineThymine);
            ResetNucleotideStates();
            adenineThymineMade++;
            Debug.Log("Adenine + Thymine = Adenine-Thymine");
        }
        else if (isGuanineIn && isCytosineIn) // G + C
        {
            Destroy(guanineInstance);
            Destroy(cytosineInstance);
            GameObject newGuanineCytosine = Instantiate(guanineCytosinePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            moleculeInstances.Add(newGuanineCytosine);
            ResetNucleotideStates();
            guanineCytosineMade++;
            Debug.Log("Guanine + Cytosine = Guanine-Cytosine");
        }

        //invalid combos
        else
        {
            if (isAdenineIn && isSecondAdenineIn) // A + A
            {
                HandleInvalidCombination(adenineInstance, adenineInstance2);
                Debug.Log("Adenine + Adenine = nothing, destroying");
            }
            else if (isGuanineIn && isSecondGuanineIn) // G + G
            {
                HandleInvalidCombination(guanineInstance, guanineInstance2);
                Debug.Log("Guanine + Guanine = nothing, destroying");
            }
            else if (isThymineIn && isSecondThymineIn) // T + T
            {
                HandleInvalidCombination(thymineInstance, thymineInstance2);
                Debug.Log("Thymine + Thymine = nothing, destroying");
            }
            else if (isCytosineIn && isSecondCytosineIn) // C + C
            {
                HandleInvalidCombination(cytosineInstance, cytosineInstance2);
                Debug.Log("Cytosine + Cytosine = nothing, destroying");
            }
            else if (isAdenineIn && isGuanineIn) // A + G
            {
                HandleInvalidCombination(adenineInstance, guanineInstance);
                Debug.Log("Adenine + Guanine = nothing, destroying");
            }
            else if (isAdenineIn && isCytosineIn) // A + C
            {
                HandleInvalidCombination(adenineInstance, cytosineInstance);
                Debug.Log("Adenine + Cytosine = nothing, destroying");
            }
            else if (isThymineIn && isGuanineIn) // T + G
            {
                HandleInvalidCombination(thymineInstance, guanineInstance);
                Debug.Log("Thymine + Guanine = nothing, destroying");
            }
            else if (isThymineIn && isCytosineIn) // T + C
            {
                HandleInvalidCombination(thymineInstance, cytosineInstance);
                Debug.Log("Thymine + Cytosine = nothing, destroying");
            }
            else if (isGuanineIn && isThymineIn) // G + T
            {
                HandleInvalidCombination(guanineInstance, thymineInstance);
                Debug.Log("Guanine + Thymine = nothing, destroying");
            }
            else if (isCytosineIn && isAdenineIn) // C + A
            {
                HandleInvalidCombination(cytosineInstance, adenineInstance);
                Debug.Log("Cytosine + Adenine = nothing, destroying");
            }
            //invalid combo (sugars + nucleotides)
            else if (isGlucoseIn && isAdenineIn)
            {
                HandleSugarInvalidCombination(glucoseInstance, adenineInstance);
                Debug.Log("Glucose + Adenine = nothing, destroying");
            }
            else if (isFructoseIn && isAdenineIn)
            {
                HandleSugarInvalidCombination(fructoseInstance, adenineInstance);
                Debug.Log("Fructose + Adenine = nothing, destroying");
            }
            else if (isGalactoseIn && isAdenineIn)
            {
                HandleSugarInvalidCombination(galactoseInstance, adenineInstance);
                Debug.Log("Galactose + Adenine = nothing, destroying");
            }
            else if (isGlucoseIn && isGuanineIn)
            {
                HandleSugarInvalidCombination(glucoseInstance, guanineInstance);
                Debug.Log("Glucose + Guanine = nothing, destroying");
            }
            else if (isFructoseIn && isGuanineIn)
            {
                HandleSugarInvalidCombination(fructoseInstance, guanineInstance);
                Debug.Log("Fructose + Guanine = nothing, destroying");
            }
            else if (isGalactoseIn && isGuanineIn)
            {
                HandleSugarInvalidCombination(galactoseInstance, guanineInstance);
                Debug.Log("Galactose + Guanine = nothing, destroying");
            }
            else if (isGlucoseIn && isCytosineIn)
            {
                HandleSugarInvalidCombination(glucoseInstance, cytosineInstance);
                Debug.Log("Glucose + Cytosine = nothing, destroying");
            }
            else if (isFructoseIn && isCytosineIn)
            {
                HandleSugarInvalidCombination(fructoseInstance, cytosineInstance);
                Debug.Log("Fructose + Cytosine = nothing, destroying");
            }
            else if (isGalactoseIn && isCytosineIn)
            {
                HandleSugarInvalidCombination(galactoseInstance, cytosineInstance);
                Debug.Log("Galactose + Cytosine = nothing, destroying");
            }
            else if (isGlucoseIn && isThymineIn)
            {
                HandleSugarInvalidCombination(glucoseInstance, thymineInstance);
                Debug.Log("Glucose + Thymine = nothing, destroying");
            }
            else if (isFructoseIn && isThymineIn)
            {
                HandleSugarInvalidCombination(fructoseInstance, thymineInstance);
                Debug.Log("Fructose + Thymine = nothing, destroying");
            }
            else if (isGalactoseIn && isThymineIn)
            {
                HandleSugarInvalidCombination(galactoseInstance, thymineInstance);
                Debug.Log("Galactose + Thymine = nothing, destroying");
            }
        }

    }
    void CreateDNA()
    {
        //valid combo
        if (isAdenineThymineIn == true && isGuanineCytosineIn == true)
        {
            Destroy(adenineThymineInstance);
            Destroy(guanineCytosineInstance);
            GameObject newDNA = Instantiate(DNAPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            moleculeInstances.Add(newDNA);
            ResetNucleotideStates();
            DNAMade++;
            Debug.Log("spawning dna");
        }

        // Invalid combos for Base Pairs
        else if (isAdenineThymineIn && isSecondAdenineThymineIn)
        {
            HandleInvalidCombination(adenineThymineInstance, adenineThymineInstance2);
            Debug.Log("Adenine-Thymine + Adenine-Thymine = nothing, destroying");
        }
        else if (isGuanineCytosineIn && isSecondGuanineCytosineIn)
        {
            HandleInvalidCombination(guanineCytosineInstance, guanineCytosineInstance2);
            Debug.Log("Guanine-Cytosine + Guanine-Cytosine = nothing, destroying");
        }
        else if (isAdenineThymineIn && isGuanineCytosineIn)
        {
            HandleInvalidCombination(adenineThymineInstance, guanineCytosineInstance);
            Debug.Log("Adenine-Thymine + Guanine-Cytosine = nothing, destroying");
        }
        else if (isAdenineThymineIn && isCytosineIn)
        {
            HandleInvalidCombination(adenineThymineInstance, cytosineInstance);
            Debug.Log("Adenine-Thymine + Cytosine = nothing, destroying");
        }
        // Invalid combos for Sugars + Base Pairs / Nucleotides
        else if (isGlucoseIn && isAdenineThymineIn)
        {
            HandleSugarInvalidCombination(glucoseInstance, adenineThymineInstance);
            Debug.Log("Glucose + Adenine-Thymine = nothing, destroying");
        }
        else if (isFructoseIn && isAdenineThymineIn)
        {
            HandleSugarInvalidCombination(fructoseInstance, adenineThymineInstance);
            Debug.Log("Fructose + Adenine-Thymine = nothing, destroying");
        }
        else if (isGalactoseIn && isAdenineThymineIn)
        {
            HandleSugarInvalidCombination(galactoseInstance, adenineThymineInstance);
            Debug.Log("Galactose + Adenine-Thymine = nothing, destroying");
        }
        else if (isGlucoseIn && isGuanineCytosineIn)
        {
            HandleSugarInvalidCombination(glucoseInstance, guanineCytosineInstance);
            Debug.Log("Glucose + Guanine-Cytosine = nothing, destroying");
        }
        else if (isFructoseIn && isGuanineCytosineIn)
        {
            HandleSugarInvalidCombination(fructoseInstance, guanineCytosineInstance);
            Debug.Log("Fructose + Guanine-Cytosine = nothing, destroying");
        }
        else if (isGalactoseIn && isGuanineCytosineIn)
        {
            HandleSugarInvalidCombination(galactoseInstance, guanineCytosineInstance);
            Debug.Log("Galactose + Guanine-Cytosine = nothing, destroying");
        }
        else if (isGlucoseIn && isAdenineIn)
        {
            HandleSugarInvalidCombination(glucoseInstance, adenineInstance);
            Debug.Log("Glucose + Adenine = nothing, destroying");
        }
        else if (isFructoseIn && isAdenineIn)
        {
            HandleSugarInvalidCombination(fructoseInstance, adenineInstance);
            Debug.Log("Fructose + Adenine = nothing, destroying");
        }
        else if (isGalactoseIn && isAdenineIn)
        {
            HandleSugarInvalidCombination(galactoseInstance, adenineInstance);
            Debug.Log("Galactose + Adenine = nothing, destroying");
        }
        else if (isGlucoseIn && isGuanineIn)
        {
            HandleSugarInvalidCombination(glucoseInstance, guanineInstance);
            Debug.Log("Glucose + Guanine = nothing, destroying");
        }
        else if (isFructoseIn && isGuanineIn)
        {
            HandleSugarInvalidCombination(fructoseInstance, guanineInstance);
            Debug.Log("Fructose + Guanine = nothing, destroying");
        }
        else if (isGalactoseIn && isGuanineIn)
        {
            HandleSugarInvalidCombination(galactoseInstance, guanineInstance);
            Debug.Log("Galactose + Guanine = nothing, destroying");
        }
        else if (isGlucoseIn && isCytosineIn)
        {
            HandleSugarInvalidCombination(glucoseInstance, cytosineInstance);
            Debug.Log("Glucose + Cytosine = nothing, destroying");
        }
        else if (isFructoseIn && isCytosineIn)
        {
            HandleSugarInvalidCombination(fructoseInstance, cytosineInstance);
            Debug.Log("Fructose + Cytosine = nothing, destroying");
        }
        else if (isGalactoseIn && isCytosineIn)
        {
            HandleSugarInvalidCombination(galactoseInstance, cytosineInstance);
            Debug.Log("Galactose + Cytosine = nothing, destroying");
        }
        else if (isGlucoseIn && isThymineIn)
        {
            HandleSugarInvalidCombination(glucoseInstance, thymineInstance);
            Debug.Log("Glucose + Thymine = nothing, destroying");
        }
        else if (isFructoseIn && isThymineIn)
        {
            HandleSugarInvalidCombination(fructoseInstance, thymineInstance);
            Debug.Log("Fructose + Thymine = nothing, destroying");
        }
        else if (isGalactoseIn && isThymineIn)
        {
            HandleSugarInvalidCombination(galactoseInstance, thymineInstance);
            Debug.Log("Galactose + Thymine = nothing, destroying");
        }

    }
    void ResetNucleotideStates()
    {
        isAdenineIn = false;
        isThymineIn = false;
        isGuanineIn = false;
        isCytosineIn = false;
        isSecondAdenineIn = false;
        isSecondThymineIn = false;
        isSecondGuanineIn = false;
        isSecondCytosineIn = false;
        isAdenineThymineIn = false;
        isSecondAdenineThymineIn = false;
        isGuanineCytosineIn = false;
        isSecondGuanineCytosineIn = false;
    }
    void HandleInvalidCombination(GameObject firstInstance, GameObject secondInstance)
    {
        Destroy(firstInstance);
        Destroy(secondInstance);
        ResetCarbohydratesStates();
        ResetNucleotideStates();
    }
    void HandleSugarInvalidCombination(GameObject sugarInstance, GameObject nucleotideInstance)
    {
        Destroy(sugarInstance);
        Destroy(nucleotideInstance);
        ResetCarbohydratesStates();
        ResetNucleotideStates();
    }
}
