using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private GameObject trainingPanel;
    [SerializeField] private GameObject jobApplicationPanel;
    [SerializeField] private TMP_Text interactionText;

    private bool isInteracting = false;
    private PlayerController playerController;
    private ObjectiveSystem objectiveSystem;
    private AudioSource audioSource;

    private void Update()
    {
        if (isInteracting)
        {
            HandleInteractionPanel();
        }
        else
        {
            CheckForInteraction();
        }
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        objectiveSystem = FindFirstObjectByType<ObjectiveSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void CheckForInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactionRange
        );

        bool foundInteractable = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Interactable"))
            {
                foundInteractable = true;

                if (Keyboard.current != null &&
                    Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Interact(collider.gameObject);
                    return;
                }

                break;
            }
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(foundInteractable);
        }
    }

    private void Interact(GameObject interactableObject)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        BuildingInteraction buildingInteraction =
            interactableObject.GetComponent<BuildingInteraction>();

        NPCInteraction npcInteraction =
            interactableObject.GetComponent<NPCInteraction>();

        TrainingComputer trainingComputer =
            interactableObject.GetComponent<TrainingComputer>();

        DocumentInteraction documentInteraction =
            interactableObject.GetComponent<DocumentInteraction>();

        if (buildingInteraction == null &&
            npcInteraction == null &&
            trainingComputer == null &&
            documentInteraction == null)
        {
            Debug.LogWarning(
                "No supported interaction component found on " +
                interactableObject.name
            );

            return;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (buildingInteraction != null)
        {
            if (interactionText != null)
            {
                interactionText.text =
                    buildingInteraction.InteractionMessage;
            }

            if (buildingInteraction.UpdatesObjective &&
                objectiveSystem != null)
            {
                objectiveSystem.SetObjective(
                    buildingInteraction.NewObjective
                );
            }

            Debug.Log(
                "Interacted with: " +
                buildingInteraction.BuildingName
            );
        }
        else if (npcInteraction != null)
        {
            if (npcInteraction.OpensJobApplication)
            {
                if (npcInteraction.ProgressionSystem != null &&
                    npcInteraction.ProgressionSystem.JobApplicationCompleted)
                {
                    if (npcInteraction.ProgressionSystem.AllDocumentsCollected())
                    {
                        if (interactionText != null)
                        {
                            interactionText.text =
                                npcInteraction.ReadyForInterviewMessage;
                        }

                        npcInteraction.ProgressionSystem.StartInterview();

                        if (objectiveSystem != null)
                        {
                            objectiveSystem.SetObjective(
                                npcInteraction.ReadyForInterviewObjective
                            );
                        }

                        if (npcInteraction.InterviewSystem != null)
                        {
                            npcInteraction.InterviewSystem.OpenInterview();
                        }

                        Debug.Log(
                            "Business Manager: Player is ready for interview."
                        );
                    }
                    else
                    {
                        if (interactionText != null)
                        {
                            interactionText.text =
                                npcInteraction.CompletedApplicationMessage;
                        }

                        if (objectiveSystem != null)
                        {
                            objectiveSystem.SetObjective(
                                npcInteraction.CompletedApplicationObjective
                            );
                        }

                        Debug.Log(
                            "Business Manager: Documents are still missing."
                        );
                    }
                }
                else
                {
                    if (jobApplicationPanel != null)
                    {
                        jobApplicationPanel.SetActive(true);
                    }

                    Debug.Log(
                        "Opened job application with: " +
                        npcInteraction.NPCName
                    );
                }
            }
            else
            {
                if (npcInteraction.ProgressionSystem != null &&
                    npcInteraction.ProgressionSystem.JobApplicationCompleted)
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            npcInteraction.CompletedApplicationMessage;
                    }

                    if (objectiveSystem != null)
                    {
                        objectiveSystem.SetObjective(
                            npcInteraction.CompletedApplicationObjective
                        );
                    }
                }
                else
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            npcInteraction.InteractionMessage;
                    }

                    if (npcInteraction.UpdatesObjective &&
                        objectiveSystem != null)
                    {
                        objectiveSystem.SetObjective(
                            npcInteraction.NewObjective
                        );
                    }
                }

                Debug.Log(
                    "Talked to: " +
                    npcInteraction.NPCName
                );
            }
        }
        else if (trainingComputer != null)
        {
            if (interactionText != null)
            {
                interactionText.text =
                    "Begin computer skills training.";
            }

            Debug.Log(
                "Interacted with: " +
                trainingComputer.ComputerName
            );
        }
        else if (documentInteraction != null)
        {
            if (interactionText != null)
            {
                interactionText.text =
                    documentInteraction.DocumentName +
                    "\n\n" +
                    documentInteraction.InteractionMessage;
            }

            documentInteraction.Collect();

            Debug.Log(
                "Collected document: " +
                documentInteraction.DocumentName
            );
        }

        if (trainingComputer != null)
        {
            trainingComputer.OpenTraining();
        }
        else if (npcInteraction != null &&
                 npcInteraction.OpensJobApplication &&
                 npcInteraction.ProgressionSystem != null &&
                 !npcInteraction.ProgressionSystem.JobApplicationCompleted)
        {
            // The job application panel is already open.
        }
        else
        {
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(true);
            }
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        isInteracting = true;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }
    }

    private void HandleInteractionPanel()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            isInteracting = false;

            if (playerController != null)
            {
                playerController.SetMovementEnabled(true);
            }
        }
    }

    public void CloseTrainingInteraction()
    {
        if (trainingPanel != null)
        {
            trainingPanel.SetActive(false);
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        isInteracting = false;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }
    }

    public void CloseJobApplication()
    {
        if (jobApplicationPanel != null)
        {
            jobApplicationPanel.SetActive(false);
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        isInteracting = false;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }
    }
}