using UnityEngine;

public class BuildingInteraction : MonoBehaviour
{
    [Header("Interaction Information")]
    [SerializeField] private string buildingName = "Shop";

    [SerializeField]
    [TextArea(2, 5)]
    private string interactionMessage =
        "There are currently no job vacancies available.";

    [Header("Objective Update")]
    [SerializeField] private bool updatesObjective = false;

    [SerializeField]
    [TextArea(2, 4)]
    private string newObjective;

    public string BuildingName => buildingName;
    public string InteractionMessage => interactionMessage;
    public bool UpdatesObjective => updatesObjective;
    public string NewObjective => newObjective;
}