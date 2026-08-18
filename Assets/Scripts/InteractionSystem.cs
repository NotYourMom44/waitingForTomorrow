using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;

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

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Interactable"))
            {
                Debug.Log("Interactable object detected: " + collider.gameObject.name);

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Interact(collider.gameObject);
                }

                break;
            }
        }
    }

    private void Interact(GameObject interactableObject)
    {
        Debug.Log("Interacted with: " + interactableObject.name);
    }
}