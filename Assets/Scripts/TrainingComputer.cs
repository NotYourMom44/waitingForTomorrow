using UnityEngine;

public class TrainingComputer : MonoBehaviour
{
    [Header("Training UI")]
    [SerializeField] private GameObject trainingPanel;

    [Header("Interaction Information")]
    [SerializeField] private string computerName = "Training Computer";

    public string ComputerName => computerName;

    public void OpenTraining()
    {
        if (trainingPanel != null)
        {
            trainingPanel.SetActive(true);
        }
    }
}