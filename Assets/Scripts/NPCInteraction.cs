using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Interaction Information")]
    [SerializeField] private string npcName = "Neighbour";

    [SerializeField]
    [TextArea(2, 5)]
    private string interactionMessage =
        "I heard there's a business nearby looking for someone.";

    [Header("Objective Update")]
    [SerializeField] private bool updatesObjective = false;

    [SerializeField]
    [TextArea(2, 4)]
    private string newObjective;

    [Header("Special Interaction")]
    [SerializeField] private bool opensJobApplication = false;

    public string NPCName => npcName;
    public string InteractionMessage => interactionMessage;
    public bool UpdatesObjective => updatesObjective;
    public string NewObjective => newObjective;
    public bool OpensJobApplication => opensJobApplication;
}