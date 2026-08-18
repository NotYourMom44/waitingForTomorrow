using UnityEngine;

public class BuildingInteraction : MonoBehaviour
{
    [Header("Interaction Information")]
    [SerializeField] private string buildingName = "Shop";
    [SerializeField]
    [TextArea(2, 5)]
    private string interactionMessage =
        "There are currently no job vacancies available.";

    public string BuildingName => buildingName;
    public string InteractionMessage => interactionMessage;
}