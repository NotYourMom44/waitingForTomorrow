using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimedTrialSystem : MonoBehaviour
{
    [Header("Trial Settings")]
    [SerializeField] private float timeLimit = 30f;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("Retry")]
    [SerializeField] private TMP_Text failureText;
    [SerializeField] private TMP_Text retryText;
    [SerializeField] private Transform player;
    [SerializeField] private Transform retrySpawnPoint;

    private float timeRemaining;
    private bool trialRunning;

    public bool TrialRunning => trialRunning;

    public bool TrialFailed { get; private set; }

    private void Start()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        if (failureText != null)
        {
            failureText.gameObject.SetActive(false);
        }

        if (retryText != null)
        {
            retryText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (TrialFailed && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            RetryTrial();
            return;
        }

        if (!trialRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            trialRunning = false;
            TrialFailed = true;

            if (timerText != null)
            {
                timerText.gameObject.SetActive(false);
            }

            if (failureText != null)
            {
                failureText.gameObject.SetActive(true);
            }

            if (retryText != null)
            {
                retryText.gameObject.SetActive(true);
            }

            PlayerController playerController =
                player != null
                    ? player.GetComponent<PlayerController>()
                    : null;

            if (playerController != null)
            {
                playerController.SetMovementEnabled(false);
            }

            Debug.Log("Timed trial failed. Waiting for retry.");

            return;
        }

        UpdateTimerText();
    }

    public void StartTrial()
    {
        TrialFailed = false;

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

    private void RetryTrial()
    {
        if (!TrialFailed)
            return;

        if (player == null || retrySpawnPoint == null)
            return;

        CharacterController characterController =
            player.GetComponent<CharacterController>();

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        player.position = retrySpawnPoint.position;
        player.rotation = retrySpawnPoint.rotation;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        if (failureText != null)
        {
            failureText.gameObject.SetActive(false);
        }

        if (retryText != null)
        {
            retryText.gameObject.SetActive(false);
        }

        PlayerController playerController =
            player.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }

        StartTrial();

        Debug.Log("Timed trial restarted.");
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time remaining: " + seconds;
    }
}