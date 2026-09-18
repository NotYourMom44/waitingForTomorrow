using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InterviewSystem : MonoBehaviour
{
    [Header("Interview UI")]
    [SerializeField] private GameObject interviewPanel;
    [SerializeField] private TMP_Text interviewQuestion;
    [SerializeField] private TMP_Text interviewTimer;
    [SerializeField] private TMP_Text interviewFeedback;

    [SerializeField] private Button answerButton1;
    [SerializeField] private Button answerButton2;
    [SerializeField] private Button answerButton3;
    [SerializeField] private Button answerButton4;

    [SerializeField] private Button retryInterviewButton;

    [Header("Interview Camera")]
    [SerializeField] private CameraController cameraController;

    [Header("Question 1")]
    [SerializeField]
    private string question1 =
        "Why do you think you'd be a good fit for this position?";

    [SerializeField]
    private string answer1 =
        "I'm willing to learn and I'm ready to work hard.";

    [SerializeField]
    private string answer2 =
        "I just need any job I can get.";

    [SerializeField]
    private string answer3 =
        "I don't think the work will be very difficult.";

    [SerializeField]
    private string answer4 =
        "My friends told me I should apply.";

    [Header("Question 2")]
    [SerializeField]
    private string question2 =
        "What would you do if you were given a task you didn't know how to complete?";

    [SerializeField]
    private string question2Answer1 =
        "I'd ask for help, learn what I need, and try again.";

    [SerializeField]
    private string question2Answer2 =
        "I'd probably leave it until someone else did it.";

    [SerializeField]
    private string question2Answer3 =
        "I'd say the task was too difficult.";

    [SerializeField]
    private string question2Answer4 =
        "I'd pretend I knew how to do it.";

    [Header("Question 3")]
    [SerializeField]
    private string question3 =
        "If you were asked to complete an important task by a certain deadline, what would you do?";

    [SerializeField]
    private string question3Answer1 =
        "I'd plan my time carefully and make sure the task was completed on time.";

    [SerializeField]
    private string question3Answer2 =
        "I'd wait until the deadline was close before starting.";

    [SerializeField]
    private string question3Answer3 =
        "I'd ask someone else to do it for me.";

    [SerializeField]
    private string question3Answer4 =
        "I'd do it when I had enough time.";

    [Header("Timer")]
    [SerializeField] private float answerTime = 10f;

    [Header("Feedback")]
    [SerializeField] private float feedbackDuration = 2f;

    private float currentTime;
    private bool timerRunning = false;
    private int currentQuestion = 1;

    private int interviewScore = 0;

    private void Start()
    {
        if (retryInterviewButton != null)
        {
            retryInterviewButton.gameObject.SetActive(false);

            retryInterviewButton.onClick.RemoveAllListeners();
            retryInterviewButton.onClick.AddListener(
                RetryInterview
            );
        }
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            timerRunning = false;

            ShowReaction(false);

            StartCoroutine(NextQuestionAfterFeedback());
        }

        if (interviewTimer != null)
        {
            interviewTimer.text =
                Mathf.CeilToInt(currentTime).ToString();
        }
    }

    public void OpenInterview()
    {
        if (interviewPanel != null)
        {
            interviewPanel.SetActive(true);
        }

        if (cameraController != null)
        {
            cameraController.SetInterviewMode(true);
        }

        currentQuestion = 1;
        interviewScore = 0;

        HideRetryButton();

        LoadQuestion1();

        Debug.Log("Interview started.");
    }

    private void LoadQuestion1()
    {
        if (interviewQuestion != null)
        {
            interviewQuestion.text = question1;
        }

        if (interviewFeedback != null)
        {
            interviewFeedback.text = "";
        }

        SetAnswerButtonText(answerButton1, answer1);
        SetAnswerButtonText(answerButton2, answer2);
        SetAnswerButtonText(answerButton3, answer3);
        SetAnswerButtonText(answerButton4, answer4);

        SetAnswerButtonListeners();

        StartTimer();
    }

    private void LoadQuestion2()
    {
        if (interviewQuestion != null)
        {
            interviewQuestion.text = question2;
        }

        if (interviewFeedback != null)
        {
            interviewFeedback.text = "";
        }

        SetAnswerButtonText(answerButton1, question2Answer1);
        SetAnswerButtonText(answerButton2, question2Answer2);
        SetAnswerButtonText(answerButton3, question2Answer3);
        SetAnswerButtonText(answerButton4, question2Answer4);

        SetAnswerButtonListeners();

        StartTimer();
    }

    private void LoadQuestion3()
    {
        if (interviewQuestion != null)
        {
            interviewQuestion.text = question3;
        }

        if (interviewFeedback != null)
        {
            interviewFeedback.text = "";
        }

        SetAnswerButtonText(answerButton1, question3Answer1);
        SetAnswerButtonText(answerButton2, question3Answer2);
        SetAnswerButtonText(answerButton3, question3Answer3);
        SetAnswerButtonText(answerButton4, question3Answer4);

        SetAnswerButtonListeners();

        StartTimer();
    }

    private void SetAnswerButtonText(
        Button button,
        string answerText
    )
    {
        if (button != null)
        {
            button.GetComponentInChildren<TMP_Text>().text =
                answerText;
        }
    }

    private void SetAnswerButtonListeners()
    {
        if (answerButton1 != null)
        {
            answerButton1.onClick.RemoveAllListeners();
            answerButton1.onClick.AddListener(
                () => SelectAnswer(1)
            );
        }

        if (answerButton2 != null)
        {
            answerButton2.onClick.RemoveAllListeners();
            answerButton2.onClick.AddListener(
                () => SelectAnswer(2)
            );
        }

        if (answerButton3 != null)
        {
            answerButton3.onClick.RemoveAllListeners();
            answerButton3.onClick.AddListener(
                () => SelectAnswer(3)
            );
        }

        if (answerButton4 != null)
        {
            answerButton4.onClick.RemoveAllListeners();
            answerButton4.onClick.AddListener(
                () => SelectAnswer(4)
            );
        }
    }

    private void StartTimer()
    {
        currentTime = answerTime;
        timerRunning = true;

        if (interviewTimer != null)
        {
            interviewTimer.text =
                Mathf.CeilToInt(currentTime).ToString();
        }
    }

    private void SelectAnswer(int answerNumber)
    {
        if (!timerRunning)
            return;

        timerRunning = false;

        bool goodAnswer = answerNumber == 1;

        if (goodAnswer)
        {
            interviewScore++;
        }

        ShowReaction(goodAnswer);

        Debug.Log(
            "Player selected interview answer: " +
            answerNumber +
            " for question " +
            currentQuestion
        );

        Debug.Log(
            "Current interview score: " +
            interviewScore
        );

        StartCoroutine(NextQuestionAfterFeedback());
    }

    private void ShowReaction(bool positive)
    {
        if (interviewFeedback == null)
            return;

        if (positive)
        {
            interviewFeedback.text =
                "The manager seems pleased with your answer.";
        }
        else
        {
            interviewFeedback.text =
                "The manager seems disappointed.";
        }
    }

    private IEnumerator NextQuestionAfterFeedback()
    {
        yield return new WaitForSeconds(feedbackDuration);

        currentQuestion++;

        if (currentQuestion == 2)
        {
            LoadQuestion2();
        }
        else if (currentQuestion == 3)
        {
            LoadQuestion3();
        }
        else if (currentQuestion > 3)
        {
            timerRunning = false;

            ShowFinalResult();

            Debug.Log(
                "Interview questions completed."
            );

            Debug.Log(
                "Final interview score: " +
                interviewScore +
                "/3"
            );
        }
    }

    private void ShowFinalResult()
    {
        if (interviewFeedback == null)
            return;

        if (interviewScore == 3)
        {
            interviewFeedback.text =
                "The manager seems impressed by your answers. " +
                "You have made a strong impression.";

            HideRetryButton();
        }
        else if (interviewScore >= 1)
        {
            interviewFeedback.text =
                "The manager seems unsure. " +
                "You gave some good answers, but there is room for improvement.";

            HideRetryButton();
        }
        else
        {
            interviewFeedback.text =
                "The manager does not seem convinced by your answers.";

            ShowRetryButton();
        }
    }

    private void ShowRetryButton()
    {
        if (retryInterviewButton != null)
        {
            retryInterviewButton.gameObject.SetActive(true);
        }
    }

    private void HideRetryButton()
    {
        if (retryInterviewButton != null)
        {
            retryInterviewButton.gameObject.SetActive(false);
        }
    }

    private void RetryInterview()
    {
        Debug.Log("Interview restarted.");

        currentQuestion = 1;
        interviewScore = 0;

        HideRetryButton();

        LoadQuestion1();
    }
}