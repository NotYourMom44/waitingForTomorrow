using UnityEngine;

public class GameProgressionSystem : MonoBehaviour
{
    public bool ComputerSkillsCompleted { get; private set; }
    public bool JobApplicationCompleted { get; private set; }

    public bool CVCollected { get; private set; }
    public bool IDCollected { get; private set; }
    public bool TrainingCertificateCollected { get; private set; }

    public bool InterviewStarted { get; private set; }

    public bool WorkshopLeadReceived { get; private set; }

    public int WorkshopPanelsCollected { get; private set; }

    public bool WorkshopCrateAssembled { get; private set; }

    public void CompleteComputerSkills()
    {
        ComputerSkillsCompleted = true;
        Debug.Log("Computer skills training marked as completed.");
    }

    public void CompleteJobApplication()
    {
        JobApplicationCompleted = true;
        Debug.Log("Job application marked as completed.");
    }

    public void CollectCV()
    {
        CVCollected = true;
        Debug.Log("CV collected.");
    }

    public void CollectID()
    {
        IDCollected = true;
        Debug.Log("ID copy collected.");
    }

    public void CollectTrainingCertificate()
    {
        TrainingCertificateCollected = true;
        Debug.Log("Training certificate collected.");
    }

    public bool AllDocumentsCollected()
    {
        return CVCollected &&
               IDCollected &&
               TrainingCertificateCollected;
    }

    public void StartInterview()
    {
        InterviewStarted = true;
        Debug.Log("Interview started.");
    }

    public void ReceiveWorkshopLead()
    {
        WorkshopLeadReceived = true;
        Debug.Log("Workshop lead received.");
    }

    public void CollectWorkshopPanel()
    {
        WorkshopPanelsCollected++;

        Debug.Log(
            "Workshop panel collected. Total: " +
            WorkshopPanelsCollected
        );
    }

    public void CompleteWorkshopAssembly()
    {
        WorkshopCrateAssembled = true;

        Debug.Log("Workshop crate assembly completed.");
    }
}