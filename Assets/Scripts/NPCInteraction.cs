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

    [Header("Progression Dialogue")]
    [SerializeField] private GameProgressionSystem progressionSystem;

    [SerializeField]
    [TextArea(2, 5)]
    private string completedApplicationMessage =
        "The business is ready to move forward with your application. Before the interview, make sure you have your CV, ID, and proof of your training ready.";

    [SerializeField]
    [TextArea(2, 4)]
    private string completedApplicationObjective =
        "Prepare your documents for the interview.";

    public string NPCName => npcName;
    public string InteractionMessage => interactionMessage;
    public bool UpdatesObjective => updatesObjective;
    public string NewObjective => newObjective;
    public bool OpensJobApplication => opensJobApplication;

    public GameProgressionSystem ProgressionSystem => progressionSystem;
    public string CompletedApplicationMessage => completedApplicationMessage;
    public string CompletedApplicationObjective => completedApplicationObjective;
}