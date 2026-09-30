using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSequence : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;

        [TextArea(2, 5)]
        public string dialogue;
    }

    [Header("Dialogue")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("UI")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;

    [Header("Ending")]
    [SerializeField]
    private string endingObjective =
        "Go home and think about the opportunity.";

    private int currentLine = 0;
    private bool dialogueActive = false;

    private InteractionSystem interactionSystem;
    private ObjectiveSystem objectiveSystem;

    public bool DialogueActive => dialogueActive;

    public bool DialogueCompleted { get; private set; }

    private void Awake()
    {
        interactionSystem =
            FindFirstObjectByType<InteractionSystem>();

        objectiveSystem =
            FindFirstObjectByType<ObjectiveSystem>();
    }

    public void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        currentLine = 0;
        dialogueActive = true;
        DialogueCompleted = false;

        ShowCurrentLine();
    }

    public void NextLine()
    {
        if (!dialogueActive)
            return;

        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (dialogueText != null)
        {
            dialogueText.text =
                dialogueLines[currentLine].speaker +
                ":\n" +
                dialogueLines[currentLine].dialogue;
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        DialogueCompleted = true;

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
        }

        if (objectiveSystem != null)
        {
            objectiveSystem.SetObjective(endingObjective);
        }

        if (interactionSystem != null)
        {
            interactionSystem.CloseDialogueInteraction();
        }

        Debug.Log("Dialogue sequence completed.");
    }
}