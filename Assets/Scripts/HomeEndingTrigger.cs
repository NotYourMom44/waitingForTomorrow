using UnityEngine;

public class HomeEndingTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogueSequence;
    [SerializeField] private FinalEndingTransition finalEndingTransition;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (dialogueSequence == null)
            return;

        if (!dialogueSequence.DialogueCompleted)
            return;

        triggered = true;

        if (finalEndingTransition != null)
        {
            finalEndingTransition.PlayEnding();
        }

        Debug.Log("Final home ending triggered.");
    }
}