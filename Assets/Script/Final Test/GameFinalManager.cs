using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameFinalManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;    // The text of the question
        public string[] choices;       // Array of choices
        public int correctAnswerIndex; // Index (0,1,2) of the correct answer
    }

    [System.Serializable]
    public class Exam
    {
        public string examTitle;           // Title of the exam
        public List<Question> questions;   // List of questions for this exam
    }

    [Header("UI References")]
    public TextMeshProUGUI examTitleText;   // Displays the exam title
    public TextMeshProUGUI questionTextUI;  // Displays the question text
    public Button[] choiceButtons;          // Buttons for answer choices
    public Button nextButton;               // Button to proceed to the next question/exam

    [Header("Exam Settings")]
    public List<Exam> exams = new List<Exam>();  // List of exam sets

    [Header("Panels")]
    public GameObject winPanel;  // Panel to open if the player scores 12/12
    public GameObject losePanel; // Panel to open if the player's overall score is 6 or below

    [Header("Button Colors")]
    public Color selectedColor = Color.green; // Color to indicate a selected answer

    // Internal variables for button color resetting
    private Color[] defaultButtonColors;

    // Tracking indices and scores
    private int currentExamIndex = 0;
    private int currentQuestionIndex = 0;
    private int examCorrectCount = 0;      // Correct answers in current exam
    private int overallCorrectCount = 0;   // Overall correct answers

    // Stores the player's current answer selection (-1 means no selection)
    private int selectedAnswerIndex = -1;
    // Reference to the currently selected button (if any)
    private Button selectedButton;
    // Flag to prevent double-clicks during question transitions
    private bool isProcessing = false;

    void Start()
    {
        // Save each button's default color so we can reset it later.
        defaultButtonColors = new Color[choiceButtons.Length];
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Image img = choiceButtons[i].GetComponent<Image>();
            if (img != null)
                defaultButtonColors[i] = img.color;
        }

        // Set up listeners for choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(index));
        }
        // Set up listener for the Next button
        nextButton.onClick.AddListener(OnNextQuestion);

        // Make sure the panels are inactive at start.
        if (winPanel != null)
            winPanel.SetActive(false);
        if (losePanel != null)
            losePanel.SetActive(false);

        if (exams.Count > 0)
        {
            StartExam();
        }
        else
        {
            Debug.LogError("No exams configured!");
        }
    }

    // Starts the current exam by resetting the question index and score.
    void StartExam()
    {
        currentQuestionIndex = 0;
        examCorrectCount = 0;
        DisplayQuestion();
    }

    // Displays the current question and resets the UI elements.
    void DisplayQuestion()
    {
        Exam currentExam = exams[currentExamIndex];

        // Ensure there is at least one question in the exam.
        if (currentExam.questions.Count == 0)
        {
            Debug.LogError("Exam " + currentExam.examTitle + " has no questions!");
            return;
        }

        Question currentQuestion = currentExam.questions[currentQuestionIndex];

        // Ensure the question has a valid choices array.
        if (currentQuestion.choices == null || currentQuestion.choices.Length == 0)
        {
            Debug.LogError("Question \"" + currentQuestion.questionText + "\" has no choices!");
            return;
        }

        // Verify that the correctAnswerIndex is within bounds.
        if (currentQuestion.correctAnswerIndex < 0 || currentQuestion.correctAnswerIndex >= currentQuestion.choices.Length)
        {
            Debug.LogError("Question \"" + currentQuestion.questionText + "\" has an invalid correctAnswerIndex (" + currentQuestion.correctAnswerIndex + ").");
            return;
        }

        // Update UI texts.
        examTitleText.text = currentExam.examTitle;
        questionTextUI.text = currentQuestion.questionText;

        // Reset selection state.
        selectedAnswerIndex = -1;
        selectedButton = null;
        isProcessing = false;

        // Update choice buttons and reset their color to default.
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentQuestion.choices.Length)
            {
                TextMeshProUGUI btnText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                    btnText.text = currentQuestion.choices[i];
                choiceButtons[i].interactable = true;
                choiceButtons[i].gameObject.SetActive(true);
                // Reset the button color to its default.
                Image img = choiceButtons[i].GetComponent<Image>();
                if (img != null)
                    img.color = defaultButtonColors[i];
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }

        nextButton.interactable = true;
        Debug.Log("Displayed question " + (currentQuestionIndex + 1) + " of exam: " + currentExam.examTitle);
    }

    // Called when a choice button is clicked.
    public void OnChoiceSelected(int index)
    {
        if (isProcessing)
            return;

        // Safety check: Ensure index is valid.
        Exam currentExam = exams[currentExamIndex];
        Question currentQuestion = currentExam.questions[currentQuestionIndex];
        if (index < 0 || index >= currentQuestion.choices.Length)
        {
            Debug.LogError("Selected index " + index + " is out of bounds for question: " + currentQuestion.questionText);
            return;
        }

        // If there was a previous selection, reset its button color.
        if (selectedButton != null)
        {
            int prevIndex = System.Array.IndexOf(choiceButtons, selectedButton);
            if (prevIndex >= 0)
            {
                Image prevImg = choiceButtons[prevIndex].GetComponent<Image>();
                if (prevImg != null)
                    prevImg.color = defaultButtonColors[prevIndex];
            }
        }

        selectedAnswerIndex = index;
        selectedButton = choiceButtons[index];

        // Change the selected button's color to indicate the selection.
        Image currentImg = selectedButton.GetComponent<Image>();
        if (currentImg != null)
            currentImg.color = selectedColor;

        Debug.Log("Choice selected: index " + index);
    }

    // Called when the Next button is clicked.
    public void OnNextQuestion()
    {
        if (isProcessing)
            return;

        // Do nothing if no answer is selected.
        if (selectedAnswerIndex == -1)
        {
            return;
        }

        isProcessing = true;
        nextButton.interactable = false;

        Exam currentExam = exams[currentExamIndex];
        Question currentQuestion = currentExam.questions[currentQuestionIndex];

        // Check if the selected answer is correct.
        if (selectedAnswerIndex == currentQuestion.correctAnswerIndex)
        {
            examCorrectCount++;
            overallCorrectCount++;
            Debug.Log("Answer is correct.");
        }
        else
        {
            Debug.Log("Answer is wrong. Correct answer: " + currentQuestion.choices[currentQuestion.correctAnswerIndex]);
        }

        // Disable all choice buttons.
        foreach (Button btn in choiceButtons)
        {
            btn.interactable = false;
        }

        Invoke("LoadNextStep", 1f);
    }

    // Moves on to the next question or shows exam results if finished.
    void LoadNextStep()
    {
        Exam currentExam = exams[currentExamIndex];
        currentQuestionIndex++; // Increment the question index

        if (currentQuestionIndex < currentExam.questions.Count)
        {
            Debug.Log("Moving to question " + (currentQuestionIndex + 1) + " of exam: " + currentExam.examTitle);
            DisplayQuestion();
        }
        else
        {
            // Exam finished: display exam results.
            questionTextUI.text = "Exam Finished!\nYou got " + examCorrectCount + " out of " + currentExam.questions.Count + " correct.";
            Debug.Log("Exam finished: " + currentExam.examTitle + ". Correct: " + examCorrectCount + "/" + currentExam.questions.Count);

            // Disable choice buttons during exam result display.
            foreach (Button btn in choiceButtons)
            {
                btn.gameObject.SetActive(false);
            }

            Invoke("MoveToNextExam", 2f);
        }
    }

    // Moves to the next exam or shows overall final results.
    void MoveToNextExam()
    {
        currentExamIndex++;
        if (currentExamIndex < exams.Count)
        {
            // Re-enable buttons for the next exam.
            foreach (Button btn in choiceButtons)
            {
                btn.gameObject.SetActive(true);
            }
            nextButton.interactable = true;
            StartExam();
            Debug.Log("Moving to next exam: " + exams[currentExamIndex].examTitle);
        }
        else
        {
            ShowFinalResults();
        }
    }

    // Calculates overall results, displays the final score,
    // and opens the win panel if full score (12/12) or the lose panel if overall score is 6 or below.
    void ShowFinalResults()
    {
        int totalQuestions = 0;
        foreach (Exam exam in exams)
        {
            totalQuestions += exam.questions.Count;
        }

        questionTextUI.text = "All exams finished!\nOverall correct: " + overallCorrectCount + " / " + totalQuestions;
        examTitleText.text = "";

        nextButton.gameObject.SetActive(false);
        Debug.Log("All exams finished. Overall correct: " + overallCorrectCount + " / " + totalQuestions);

        // Check win or lose conditions.
        // (Assuming a full score is 12/12 and a score of 6 or below is a loss.)
        if (overallCorrectCount == totalQuestions && totalQuestions == 12)
        {
            if (winPanel != null)
                winPanel.SetActive(true);
        }
        else if (overallCorrectCount <= 6)
        {
            if (losePanel != null)
                losePanel.SetActive(true);
        }
    }
}
