using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenFadeTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private TMP_Text transitionText;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float messageDuration = 1.5f;

    [SerializeField]
    private string nextMorningObjective =
        "Get ready to leave for the building supplies shop.";

    private ObjectiveSystem objectiveSystem;
    private PlayerController playerController;

    private void Awake()
    {
        objectiveSystem = FindFirstObjectByType<ObjectiveSystem>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void PlayNextMorningTransition()
    {
        StartCoroutine(NextMorningSequence());
    }

    private IEnumerator NextMorningSequence()
    {
        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        if (transitionText != null)
        {
            transitionText.gameObject.SetActive(false);
        }

        yield return StartCoroutine(Fade(0f, 1f));

        if (transitionText != null)
        {
            transitionText.text = "The next morning...";
            transitionText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(messageDuration);

        if (transitionText != null)
        {
            transitionText.gameObject.SetActive(false);
        }

        yield return StartCoroutine(Fade(1f, 0f));

        GameProgressionSystem progressionSystem =
        FindFirstObjectByType<GameProgressionSystem>();

        if (progressionSystem != null)
        {
            progressionSystem.MarkNextMorningStarted();
        }

        if (objectiveSystem != null)
        {
            objectiveSystem.SetObjective(nextMorningObjective);
        }

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float progress =
                Mathf.Clamp01(elapsed / fadeDuration);

            if (fadePanel != null)
            {
                fadePanel.alpha =
                    Mathf.Lerp(
                        startAlpha,
                        endAlpha,
                        progress
                    );
            }

            yield return null;
        }

        if (fadePanel != null)
        {
            fadePanel.alpha = endAlpha;
        }
    }
}