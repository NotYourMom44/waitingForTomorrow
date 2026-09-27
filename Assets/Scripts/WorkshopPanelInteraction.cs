using UnityEngine;

public class WorkshopPanelInteraction : MonoBehaviour
{
    [Header("Panel Information")]
    [SerializeField] private string panelName = "Wooden Panel";

    [SerializeField]
    [TextArea(2, 4)]
    private string interactionMessage =
        "Wooden panel collected.";

    [SerializeField]
    [TextArea(2, 4)]
    private string assemblyObjective =
    "Assemble the crate at the workbench.";

    [Header("Progression")]
    [SerializeField] private GameProgressionSystem progressionSystem;

    [Header("Objective")]
    [SerializeField] private ObjectiveSystem objectiveSystem;

    [SerializeField]
    [TextArea(2, 4)]
    private string collectionObjective =
        "Collect the wooden panels.";

    public string PanelName => panelName;
    public string InteractionMessage => interactionMessage;
    public GameProgressionSystem ProgressionSystem => progressionSystem;

    public ObjectiveSystem ObjectiveSystem => objectiveSystem;
    public string CollectionObjective => collectionObjective;

    public void Collect()
    {
        if (progressionSystem != null)
        {
            progressionSystem.CollectWorkshopPanel();

            if (progressionSystem.WorkshopPanelsCollected >= 3)
            {
                if (objectiveSystem != null)
                {
                    objectiveSystem.SetObjective(assemblyObjective);
                }
            }
            else
            {
                if (objectiveSystem != null)
                {
                    objectiveSystem.SetObjective(collectionObjective);
                }
            }
        }

        gameObject.SetActive(false);
    }
}