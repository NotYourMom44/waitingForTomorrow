using UnityEngine;

public class HomeDepartureTrigger : MonoBehaviour
{
    [SerializeField] private TimedTrialSystem timedTrialSystem;
    [SerializeField] private GameProgressionSystem progressionSystem;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (progressionSystem == null)
            return;

        if (!progressionSystem.NextMorningStarted)
            return;

        if (timedTrialSystem == null)
            return;

        triggered = true;

        timedTrialSystem.StartTrial();

        Debug.Log("Timed trial started from home departure.");
    }
}