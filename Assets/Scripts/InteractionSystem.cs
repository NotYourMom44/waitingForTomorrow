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
    private MoneyManager moneyManager;

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
        moneyManager = FindFirstObjectByType<MoneyManager>();
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

        WorkshopPanelInteraction workshopPanelInteraction =
            interactableObject.GetComponent<WorkshopPanelInteraction>();

        WorkshopWorkbenchInteraction workshopWorkbenchInteraction =
            interactableObject.GetComponent<WorkshopWorkbenchInteraction>();

        if (buildingInteraction == null &&
            npcInteraction == null &&
            trainingComputer == null &&
            documentInteraction == null &&
            workshopPanelInteraction == null &&
            workshopWorkbenchInteraction == null)
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
            if (npcInteraction.UsesDialogueSequence)
            {
                if (npcInteraction.DialogueSequence != null)
                {
                    npcInteraction.DialogueSequence.StartDialogue();
                }

                Debug.Log(
                    "Started dialogue sequence with: " +
                    npcInteraction.NPCName
                );
            }
            else if (npcInteraction.HandlesDocumentPayment)
            {
                if (npcInteraction.ProgressionSystem != null &&
                    npcInteraction.ProgressionSystem.DocumentExpensePaid)
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            "Your documents have already been printed and copied.";
                    }

                    Debug.Log(
                        "Print Shop Clerk: Document printing and copying has already been paid for."
                    );
                }
                else if (moneyManager != null &&
                         moneyManager.CanAfford(50))
                {
                    moneyManager.SpendMoney(50);

                    if (npcInteraction.ProgressionSystem != null)
                    {
                        npcInteraction.ProgressionSystem.MarkDocumentExpensePaid();
                        npcInteraction.ProgressionSystem.UnlockNewJobOpportunity();
                    }

                    if (objectiveSystem != null)
                    {
                        objectiveSystem.SetObjective(
                            npcInteraction.NewJobOpportunityObjective
                        );
                    }

                    if (interactionText != null)
                    {
                        interactionText.text =
                            "Sure, I can print and copy those for you. It'll be R50.\n\n" +
                            "There you go. That's everything.";
                    }

                    Debug.Log(
                        "Print Shop Clerk: Player paid R50 for document printing and copying."
                    );
                }
                else
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            "Sorry, you need R50 for the printing and copying.";
                    }

                    Debug.Log(
                        "Print Shop Clerk: Player cannot afford the R50 document expense."
                    );
                }
            }
            else if (npcInteraction.OpensJobApplication)
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
            else if (npcInteraction.CompletesWorkshopTask)
            {
                if (npcInteraction.ProgressionSystem != null &&
                    npcInteraction.ProgressionSystem.WorkshopCrateAssembled)
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            npcInteraction.WorkshopCompletionMessage;
                    }

                    if (objectiveSystem != null)
                    {
                        objectiveSystem.SetObjective(
                            npcInteraction.WorkshopCompletionObjective
                        );
                    }

                    if (!npcInteraction.ProgressionSystem.WorkshopPaymentReceived)
                    {
                        if (moneyManager != null)
                        {
                            moneyManager.AddMoney(250);
                        }

                        npcInteraction.ProgressionSystem.MarkWorkshopPaymentReceived();

                        Debug.Log(
                            "Workshop Owner: Player was paid R250 for the completed crate."
                        );
                    }
                    else
                    {
                        Debug.Log(
                            "Workshop Owner: Workshop payment has already been received."
                        );
                    }

                    Debug.Log(
                        "Workshop Owner: Player completed the practical workshop task."
                    );
                }
                else
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            "Keep working on the practical task.";
                    }

                    Debug.Log(
                        "Workshop Owner: Practical workshop task is not completed yet."
                    );
                }
            }
            else if (npcInteraction.ProvidesWorkshopLead)
            {
                if (npcInteraction.ProgressionSystem != null &&
                    npcInteraction.ProgressionSystem.WorkshopLeadReceived)
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            "I hope the workshop can help you gain some practical experience.";
                    }

                    Debug.Log(
                        "Workshop Owner: Workshop lead has already been received."
                    );
                }
                else
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            npcInteraction.InteractionMessage;
                    }

                    if (npcInteraction.ProgressionSystem != null)
                    {
                        npcInteraction.ProgressionSystem.ReceiveWorkshopLead();
                    }

                    if (objectiveSystem != null &&
                        npcInteraction.UpdatesObjective)
                    {
                        objectiveSystem.SetObjective(
                            npcInteraction.NewObjective
                        );
                    }

                    Debug.Log(
                        "Workshop Owner: Player received the workshop lead."
                    );
                }
            }
            else
            {
                if (npcInteraction.ProgressionSystem != null &&
                    npcInteraction.ProgressionSystem.BuildingSuppliesTrialCompleted &&
                    npcInteraction.IsBuildingSuppliesOwner)
                {
                    if (interactionText != null)
                    {
                        interactionText.text =
                            npcInteraction.BuildingSuppliesCompletedMessage;
                    }

                    if (objectiveSystem != null)
                    {
                        objectiveSystem.SetObjective(
                            npcInteraction.BuildingSuppliesCompletedObjective
                        );
                    }

                    npcInteraction.ActivateCommunityEmploymentContact();

                    Debug.Log(
                        "Building Supplies Owner: Player has successfully completed the timed trial."
                    );
                }
                else if (npcInteraction.ProgressionSystem != null &&
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
        else if (workshopPanelInteraction != null)
        {
            if (interactionText != null)
            {
                interactionText.text =
                    workshopPanelInteraction.PanelName +
                    "\n\n" +
                    workshopPanelInteraction.InteractionMessage;
            }

            workshopPanelInteraction.Collect();

            Debug.Log(
                "Collected workshop panel: " +
                workshopPanelInteraction.PanelName
            );
        }
        else if (workshopWorkbenchInteraction != null)
        {
            if (interactionText != null)
            {
                interactionText.text =
                    workshopWorkbenchInteraction.WorkbenchName +
                    "\n\n" +
                    workshopWorkbenchInteraction.InteractionMessage;
            }

            if (workshopWorkbenchInteraction.AssemblySystem != null)
            {
                workshopWorkbenchInteraction.AssemblySystem.OpenAssembly();
            }

            Debug.Log(
                "Opened crate assembly at workbench: " +
                workshopWorkbenchInteraction.WorkbenchName
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
        else if (workshopWorkbenchInteraction == null)
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

    public void CloseWorkshopAssembly()
    {
        isInteracting = false;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        Debug.Log("Workshop assembly interaction closed.");
    }

    public void ShowMessage(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }

        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
        }

        isInteracting = true;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }
    }

    public void CloseDialogueInteraction()
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

        Debug.Log("Dialogue interaction closed.");
    }
}