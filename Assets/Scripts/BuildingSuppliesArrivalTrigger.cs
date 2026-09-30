using UnityEngine;

public class BuildingSuppliesArrivalTrigger : MonoBehaviour
{
    [SerializeField] private TimedTrialSystem timedTrialSystem;

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

        Debug.Log("Building supplies shop reached in time.");
    }
}