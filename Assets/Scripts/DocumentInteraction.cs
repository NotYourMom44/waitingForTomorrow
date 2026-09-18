using UnityEngine;

public class DocumentInteraction : MonoBehaviour
{
    public enum DocumentType
    {
        CV,
        ID,
        TrainingCertificate
    }

    [Header("Document Information")]
    [SerializeField] private DocumentType documentType;

    [SerializeField]
    private string documentName = "Document";

    [SerializeField]
    [TextArea(2, 4)]
    private string interactionMessage =
        "You found an important document.";

    [Header("Progression")]
    [SerializeField] private GameProgressionSystem progressionSystem;

    public DocumentType Type => documentType;
    public string DocumentName => documentName;
    public string InteractionMessage => interactionMessage;
    public GameProgressionSystem ProgressionSystem => progressionSystem;

    public void Collect()
    {
        if (progressionSystem == null)
        {
            Debug.LogWarning(
                "No GameProgressionSystem assigned to " +
                gameObject.name
            );

            return;
        }

        switch (documentType)
        {
            case DocumentType.CV:
                progressionSystem.CollectCV();
                break;

            case DocumentType.ID:
                progressionSystem.CollectID();
                break;

            case DocumentType.TrainingCertificate:
                progressionSystem.CollectTrainingCertificate();
                break;
        }

        gameObject.SetActive(false);
    }
}