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

        if (buildingInteraction == null && npcInteraction == null &&
            trainingComputer == null)
        {
            Debug.LogWarning(
                "No BuildingInteraction, NPCInteraction or TrainingComputer component found on " +
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
                interactionText.text = buildingInteraction.InteractionMessage;
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
            if (interactionText != null)
            {
                interactionText.text = npcInteraction.InteractionMessage;
            }

            if (npcInteraction.UpdatesObjective &&
                objectiveSystem != null)
            {
                objectiveSystem.SetObjective(
                    npcInteraction.NewObjective
                );
            }

            Debug.Log(
                "Talked to: " +
                npcInteraction.NPCName
            );
        }

        else if (trainingComputer != null)
        {
            if (interactionText != null)
            {
                interactionText.text = "Begin computer skills training.";
            }

            Debug.Log(
                "Interacted with: " +
                trainingComputer.ComputerName
            );
        }

        if (trainingComputer != null)
        {
            trainingComputer.OpenTraining();
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
}