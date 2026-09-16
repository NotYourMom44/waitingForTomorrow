using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JobApplicationSystem : MonoBehaviour
{
    [Header("Application UI")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text feedbackText;

    [SerializeField] private Button yesButton;
    [SerializeField] private Button learningButton;

    [Header("Completion")]
    [SerializeField] private GameObject continueButton;

    [SerializeField] private ObjectiveSystem objectiveSystem;
    [SerializeField] private InteractionSystem interactionSystem;
    [SerializeField] private GameProgressionSystem progressionSystem;

    [SerializeField]
    private string completedObjective =
        "Return to the Community Centre to prepare for the next step.";

    private bool applicationCanComplete = false;

    private void Start()
    {
        ResetApplication();
    }

    private void ResetApplication()
    {
        if (questionText != null)
        {
            questionText.text =
                "We're looking for someone to help with basic administrative work.\n\n" +
                "Do you have the basic computer skills needed for the position?";
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        if (yesButton != null)
        {
            yesButton.interactable = true;
        }

        if (learningButton != null)
        {
            learningButton.interactable = true;
        }

        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }
    }

    public void SelectCompletedTraining()
    {
        applicationCanComplete = true;

        if (feedbackText != null)
        {
            feedbackText.text =
                "Good. Your computer skills training meets the basic requirement. " +
                "We can continue with your application.";
        }

        DisableChoices();

        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }
    }

    public void SelectWillingToLearn()
    {
        applicationCanComplete = false;

        if (feedbackText != null)
        {
            feedbackText.text =
                "Being willing to learn is important, but you'll need to demonstrate " +
                "the required computer skills before we can continue with your application.";
        }

        DisableChoices();

        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }
    }

    private void DisableChoices()
    {
        if (yesButton != null)
        {
            yesButton.interactable = false;
        }

        if (learningButton != null)
        {
            learningButton.interactable = false;
        }
    }

    public void ContinueApplication()
    {
        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }

        if (objectiveSystem != null)
        {
            objectiveSystem.SetObjective(completedObjective);
        }

        if (interactionSystem != null)
        {
            interactionSystem.CloseJobApplication();
        }

        if (applicationCanComplete &&
            progressionSystem != null)
        {
            progressionSystem.CompleteJobApplication();
        }

        Debug.Log("Job application completed.");
    }
}