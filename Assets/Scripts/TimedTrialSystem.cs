using TMPro;
using UnityEngine;

public class TimedTrialSystem : MonoBehaviour
{
    [Header("Trial Settings")]
    [SerializeField] private float timeLimit = 30f;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    private float timeRemaining;
    private bool trialRunning;

    public bool TrialRunning => trialRunning;

    private void Start()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!trialRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            trialRunning = false;

            if (timerText != null)
            {
                timerText.text = "Time's up! Return home and try again.";
            }

            Debug.Log("Timed trial failed.");
            return;
        }

        UpdateTimerText();
    }

    public void StartTrial()
    {
        timeRemaining = timeLimit;
        trialRunning = true;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        UpdateTimerText();
        Debug.Log("Timed trial started.");
    }

    public void CompleteTrial()
    {
        if (!trialRunning)
            return;

        trialRunning = false;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        Debug.Log("Timed trial completed.");
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time remaining: " + seconds;
    }
}