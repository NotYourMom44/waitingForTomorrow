using UnityEngine;
using TMPro;

public class ObjectiveSystem : MonoBehaviour
{
    [Header("Objective UI")]
    [SerializeField] private TMP_Text objectiveText;

    [Header("Starting Objective")]
    [TextArea(2, 4)]
    [SerializeField]
    private string startingObjective =
        "Find someone who can help you find work.";

    [Header("Document Progression")]
    [SerializeField] private GameProgressionSystem progressionSystem;

    [TextArea(2, 4)]
    [SerializeField]
    private string documentsObjective =
        "Find your CV, ID copy, and training certificate.";

    [TextArea(2, 4)]
    [SerializeField]
    private string documentsCompletedObjective =
        "Return to the business for your interview.";

    private void Start()
    {
        SetObjective(startingObjective);
    }

    private void Update()
    {
        if (progressionSystem == null)
            return;

        if (progressionSystem.InterviewStarted)
            return;

        if (progressionSystem.AllDocumentsCollected())
        {
            SetObjective(documentsCompletedObjective);
        }
    }

    public void SetObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "Objective:\n" + newObjective;
        }
    }

    public void SetDocumentsObjective()
    {
        SetObjective(documentsObjective);
    }
}