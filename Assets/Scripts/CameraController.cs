using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("First Person Camera")]
    [SerializeField] private Transform head;

    [Header("Interview Camera")]
    [SerializeField] private Transform interviewCameraPosition;

    private bool interviewMode = false;

    private void LateUpdate()
    {
        if (interviewMode)
        {
            if (interviewCameraPosition == null)
                return;

            transform.position =
                interviewCameraPosition.position;

            transform.rotation =
                interviewCameraPosition.rotation;

            return;
        }

        if (head == null)
            return;

        transform.position = head.position;
    }

    public void SetInterviewMode(bool enabled)
    {
        interviewMode = enabled;
    }
}