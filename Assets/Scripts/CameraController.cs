using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("First Person Camera")]
    [SerializeField]
    private Vector3 firstPersonOffset =
        new Vector3(0f, 3.8f, 0.35f);

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

        if (player == null)
            return;

        transform.position =
            player.TransformPoint(firstPersonOffset);
    }

    public void SetInterviewMode(bool enabled)
    {
        interviewMode = enabled;
    }
}