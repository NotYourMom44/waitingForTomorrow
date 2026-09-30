using UnityEngine;

public class HomeNextMorningTrigger : MonoBehaviour
{
    [SerializeField] private ScreenFadeTransition fadeTransition;
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

        if (!progressionSystem.NewJobOpportunityUnlocked)
            return;

        triggered = true;

        if (fadeTransition != null)
        {
            fadeTransition.PlayNextMorningTransition();
        }

        Debug.Log("Next morning sequence started.");
    }
}