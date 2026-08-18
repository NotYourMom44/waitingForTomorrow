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

    private void Start()
    {
        SetObjective(startingObjective);
    }

    public void SetObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "Objective:\n" + newObjective;
        }
    }
}