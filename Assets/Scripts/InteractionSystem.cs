using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;

    private void Update()
    {
        CheckForInteraction();
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
        Debug.Log("Interacted with: " + interactableObject.name);
    }
}