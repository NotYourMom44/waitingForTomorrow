using UnityEngine;

public class GameProgressionSystem : MonoBehaviour
{
    public bool ComputerSkillsCompleted { get; private set; }
    public bool JobApplicationCompleted { get; private set; }

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
}