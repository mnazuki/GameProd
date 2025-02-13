using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    public Text scoreText;
    public int playerScore;
    public GameObject gameOverScreen;

    public string[] taskColors = { "purple", "orange", "green" };
    private int[] colorsNeeded = { 2, 2, 3 };
    public int currentTaskIndex = 0;
    
    private string colorNeeded;
    private int amountNeeded;
    private int currentAmount;

    private MoleculeSpawner moleculeSpawner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        taskText = GameObject.Find("Task").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        moleculeSpawner = new MoleculeSpawner();
        moleculeSpawner.spawnRate = moleculeSpawner.minSpawnRate;

        ShuffleTasks();

        if (taskText != null)
        {
            NewTask(taskColors[0], colorsNeeded[0]);
        }
        if (scoreText != null)
        {
            scoreText.text = playerScore.ToString();
        }
    }

    public void AddScore()
    {
        if (moleculeSpawner.isGameFinished == false)
        {
            playerScore = playerScore + 1;
            scoreText.text = playerScore.ToString();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }

    //takes in string and integer and assigns colorNeeded and amountNeeded to color and amount respectively
    //this is then called onto the start function, and assigns it there
    public void NewTask(string color, int amount)
    {

        colorNeeded = color;
        amountNeeded = amount;
        currentAmount = 0;

        UpdateTaskText();
    }

    void UpdateTaskText()
    {
        taskText.text = "Task: Create " + amountNeeded + " " + colorNeeded + " molecules\n"
            + "Created so far: " + currentAmount + "/" + amountNeeded;
    }

    //referenced in merge.cs
    //takes in a string of "color" (e.g purple)
    public void MoleculeCreated(string color)
    {
        //if the merge matches the required color
        if (color == colorNeeded)
        {
            //adjust the currentamount
            currentAmount++;
            //adds player score
            AddScore();
            //update UI
            UpdateTaskText();

            //if our current amount exceeds or is equal the current amountneeded, then proceed with taskcomplete method
            //current amount compares itself to colorsNeeded
            //check NewTask()
            if (currentAmount >= amountNeeded)
            {
                TaskComplete();
            }
        }
    }
    void TaskComplete()
    {
        Debug.Log("Task Complete!");
        //if task completes, go up the array
        currentTaskIndex++;
        if (currentTaskIndex < taskColors.Length)
        {
            //NewTask() function takes in taskColors (string), and colorsNeeded (int)
            //currentTaskIndex takes in where we are in the array
            NewTask(taskColors[currentTaskIndex], colorsNeeded[currentTaskIndex]);

            //changes the task text 
            taskText.text = "Task: Create " + amountNeeded + " " + colorNeeded + " molecules\n" +
                           "Created so far: " + currentAmount + "/" + amountNeeded;
        }
        else
        {
            //all tasks complete
            taskText.text = "All Tasks Complete!";
        }
    }

    void ShuffleTasks()
    {
        for (int i = taskColors.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            //to swap colors
            string tempColor = taskColors[i];
            taskColors[i] = taskColors[randomIndex];
            taskColors[randomIndex] = tempColor;

            //swap amounts 
            int tempAmount = colorsNeeded[i];
            colorsNeeded[i] = colorsNeeded[randomIndex];
            colorsNeeded[randomIndex] = tempAmount;
        }
    }
}
