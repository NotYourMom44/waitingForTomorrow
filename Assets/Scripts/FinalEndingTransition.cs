using System.Collections;
using TMPro;
using UnityEngine;

public class FinalEndingTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private TMP_Text endingText;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float messageDuration = 3f;

    [SerializeField]
    private string endingMessage =
        "Johannesburg...\n\nMaybe this is the opportunity I've been waiting for.";

    [SerializeField]
    private string titleMessage =
        "WAITING FOR TOMORROW\n\nTO BE CONTINUED";

    public void PlayEnding()
    {
        StartCoroutine(EndingSequence());
    }

    private IEnumerator EndingSequence()
    {
        PlayerController playerController =
            FindFirstObjectByType<PlayerController>();

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        if (endingText != null)
        {
            endingText.gameObject.SetActive(false);
        }

        yield return StartCoroutine(Fade(0f, 1f));

        if (endingText != null)
        {
            endingText.text = endingMessage;
            endingText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(messageDuration);

        if (endingText != null)
        {
            endingText.text = titleMessage;
        }

        yield return new WaitForSeconds(messageDuration);
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