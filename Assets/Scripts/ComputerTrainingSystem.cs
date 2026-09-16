using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComputerTrainingSystem : MonoBehaviour
{
    [Header("Training Panels")]
    [SerializeField] private GameObject trainingStartPanel;
    [SerializeField] private GameObject fileOrganisationPanel;

    [Header("File Buttons")]
    [SerializeField] private Button cvButton;
    [SerializeField] private Button idButton;
    [SerializeField] private Button certificateButton;

    [Header("Folder Buttons")]
    [SerializeField] private Button personalButton;
    [SerializeField] private Button educationButton;
    [SerializeField] private Button documentsButton;

    [Header("Feedback")]
    [SerializeField] private TMP_Text feedbackText;

    [Header("Completion")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private InteractionSystem interactionSystem;

    [Header("Training Completion")]
    [SerializeField] private ObjectiveSystem objectiveSystem;
    [SerializeField] private PlayerController playerController;

    [SerializeField]
    private string completedObjective =
        "Apply for the position at the local business.";

    private string selectedFile = "";

    private int correctAnswers = 0;

    public void BeginTraining()
    {
        if (trainingStartPanel != null)
        {
            trainingStartPanel.SetActive(false);
        }

        if (fileOrganisationPanel != null)
        {
            fileOrganisationPanel.SetActive(true);
        }

        ResetTraining();

        Debug.Log("Computer training started.");
    }

    private void ResetTraining()
    {
        selectedFile = "";
        correctAnswers = 0;

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        SetFileButtonsInteractable(true);

        SetFolderButtonsInteractable(true);

        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }
    }

    public void SelectCV()
    {
        SelectFile("CV");
    }

    public void SelectID()
    {
        SelectFile("ID");
    }

    public void SelectCertificate()
    {
        SelectFile("Certificate");
    }

    private void SelectFile(string file)
    {
        selectedFile = file;

        if (feedbackText != null)
        {
            feedbackText.text = "Now select the folder for " + file + ".";
        }
    }

    public void SelectPersonal()
    {
        CheckAnswer("Personal");
    }

    public void SelectEducation()
    {
        CheckAnswer("Education");
    }

    public void SelectDocuments()
    {
        CheckAnswer("Documents");
    }

    private void CheckAnswer(string folder)
    {
        if (selectedFile == "")
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Select a file first.";
            }

            return;
        }

        bool correct = false;

        if (selectedFile == "CV" && folder == "Documents")
        {
            correct = true;
        }
        else if (selectedFile == "ID" && folder == "Personal")
        {
            correct = true;
        }
        else if (selectedFile == "Certificate" && folder == "Education")
        {
            correct = true;
        }

        if (correct)
        {
            correctAnswers++;

            if (feedbackText != null)
            {
                feedbackText.text = "Correct!";
            }

            DisableSelectedFile();

            selectedFile = "";

            if (correctAnswers >= 3)
            {
                CompleteTraining();
            }
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "That's not the correct folder. Try again.";
            }
        }
    }

    private void DisableSelectedFile()
    {
        if (selectedFile == "CV" && cvButton != null)
        {
            cvButton.interactable = false;
        }
        else if (selectedFile == "ID" && idButton != null)
        {
            idButton.interactable = false;
        }
        else if (selectedFile == "Certificate" && certificateButton != null)
        {
            certificateButton.interactable = false;
        }
    }

    private void SetFileButtonsInteractable(bool interactable)
    {
        if (cvButton != null)
            cvButton.interactable = interactable;

        if (idButton != null)
            idButton.interactable = interactable;

        if (certificateButton != null)
            certificateButton.interactable = interactable;
    }

    private void SetFolderButtonsInteractable(bool interactable)
    {
        if (personalButton != null)
            personalButton.interactable = interactable;

        if (educationButton != null)
            educationButton.interactable = interactable;

        if (documentsButton != null)
            documentsButton.interactable = interactable;
    }

    private void CompleteTraining()
    {
        if (feedbackText != null)
        {
            feedbackText.text =
                "Training complete! You have demonstrated basic computer skills.";
        }

        SetFileButtonsInteractable(false);
        SetFolderButtonsInteractable(false);

        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }

        Debug.Log("Computer training completed.");
    }

    public void ContinueTraining()
    {
        if (trainingStartPanel != null)
        {
            trainingStartPanel.SetActive(true);
        }

        if (fileOrganisationPanel != null)
        {
            fileOrganisationPanel.SetActive(false);
        }

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
            interactionSystem.CloseTrainingInteraction();
        }

        Debug.Log("Computer training closed.");
    }
}