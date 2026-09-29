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
    [SerializeField] private bool providesWorkshopLead = false;
    [SerializeField] private bool handlesDocumentPayment = false;

    [SerializeField] private bool completesWorkshopTask = false;

    [Header("Progression Dialogue")]
    [SerializeField] private GameProgressionSystem progressionSystem;

    [Header("Workshop Completion Dialogue")]
    [SerializeField]
    [TextArea(2, 5)]
    private string workshopCompletionMessage =
    "Good work. You followed the instructions and completed the job. That's the kind of practical experience you can build on.";

    [SerializeField]
    [TextArea(2, 4)]
    private string workshopCompletionObjective =
        "Continue looking for opportunities to gain work experience.";

    [SerializeField]
    [TextArea(2, 5)]
    private string completedApplicationMessage =
        "The business is ready to move forward with your application. Before the interview, make sure you have your CV, ID, and proof of your training ready.";

    [SerializeField]
    [TextArea(2, 4)]
    private string completedApplicationObjective =
        "Prepare your documents for the interview.";

    [SerializeField]
    [TextArea(2, 4)]
    private string newJobOpportunityObjective =
    "Look for the new job opportunity.";

    [Header("Interview Progression")]
    [SerializeField]
    [TextArea(2, 5)]
    private string readyForInterviewMessage =
        "Good, you have everything you need. Let's begin the interview.";

    [SerializeField]
    [TextArea(2, 4)]
    private string readyForInterviewObjective =
        "Complete your interview.";

    [Header("Interview System")]
    [SerializeField] private InterviewSystem interviewSystem;

    public InterviewSystem InterviewSystem => interviewSystem;

    public string NPCName => npcName;
    public string InteractionMessage => interactionMessage;
    public bool UpdatesObjective => updatesObjective;
    public string NewObjective => newObjective;
    public bool OpensJobApplication => opensJobApplication;
    public bool ProvidesWorkshopLead => providesWorkshopLead;

    public bool HandlesDocumentPayment => handlesDocumentPayment;

    public bool CompletesWorkshopTask => completesWorkshopTask;
    public GameProgressionSystem ProgressionSystem => progressionSystem;
    public string CompletedApplicationMessage => completedApplicationMessage;
    public string CompletedApplicationObjective => completedApplicationObjective;

    public string NewJobOpportunityObjective => newJobOpportunityObjective;

    public string WorkshopCompletionMessage => workshopCompletionMessage;
    public string WorkshopCompletionObjective => workshopCompletionObjective;
    public string ReadyForInterviewMessage => readyForInterviewMessage;
    public string ReadyForInterviewObjective => readyForInterviewObjective;
}