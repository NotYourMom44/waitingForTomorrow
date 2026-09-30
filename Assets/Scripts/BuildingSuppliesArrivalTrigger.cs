using UnityEngine;

public class BuildingSuppliesArrivalTrigger : MonoBehaviour
{
    [SerializeField] private TimedTrialSystem timedTrialSystem;
    [SerializeField] private ObjectiveSystem objectiveSystem;

    [SerializeField] private GameProgressionSystem progressionSystem;

    [SerializeField]
    private string successfulObjective =
        "Speak to the building supplies owner.";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (timedTrialSystem == null)
            return;

        if (!timedTrialSystem.TrialRunning)
            return;

        triggered = true;

        timedTrialSystem.CompleteTrial();

        if (progressionSystem != null)
        {
            progressionSystem.MarkBuildingSuppliesTrialCompleted();
        }

        if (objectiveSystem != null)
        {
            objectiveSystem.SetObjective(successfulObjective);
        }

        Debug.Log("Building supplies shop reached in time.");
    }
}