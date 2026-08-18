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

        if (buildingInteraction == null)
        {
            Debug.LogWarning(
                "No BuildingInteraction component found on " +
                interactableObject.name
            );

            return;
        }

        if (interactionText != null)
        {
            interactionText.text = buildingInteraction.InteractionMessage;
        }

        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        if (buildingInteraction.UpdatesObjective &&
    objectiveSystem != null)
        {
            objectiveSystem.SetObjective(
                buildingInteraction.NewObjective
            );
        }

        isInteracting = true;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        Debug.Log("Interacted with: " + buildingInteraction.BuildingName);
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