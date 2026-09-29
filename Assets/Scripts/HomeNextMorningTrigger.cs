using UnityEngine;

public class HomeNextMorningTrigger : MonoBehaviour
{
    [SerializeField] private ScreenFadeTransition fadeTransition;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        if (fadeTransition != null)
        {
            fadeTransition.PlayNextMorningTransition();
        }

        Debug.Log("Next morning sequence started.");
    }
}