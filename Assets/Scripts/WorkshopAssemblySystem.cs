using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkshopAssemblySystem : MonoBehaviour
{
    [Header("Assembly UI")]
    [SerializeField] private GameObject assemblyPanel;
    [SerializeField] private TMP_Text assemblyInstruction;

    [SerializeField] private Button assemblyButton1;
    [SerializeField] private Button assemblyButton2;
    [SerializeField] private Button assemblyButton3;

    [SerializeField] private Button assemblyContinueButton;

    [Header("Progression")]
    [SerializeField] private GameProgressionSystem progressionSystem;

    [SerializeField] private InteractionSystem interactionSystem;

    [Header("Objective")]
    [SerializeField] private ObjectiveSystem objectiveSystem;

    [Header("Finished Crate")]
    [SerializeField] private GameObject finishedCrate;

    [SerializeField]
    [TextArea(2, 4)]
    private string completedObjective =
        "Show your work to the Workshop Owner.";

    private int currentStep = 1;

    public void OpenAssembly()
    {
        currentStep = 1;

        if (assemblyPanel != null)
        {
            assemblyPanel.SetActive(true);
        }

        UpdateInstruction();

        if (assemblyContinueButton != null)
        {
            assemblyContinueButton.gameObject.SetActive(false);
        }

        SetupButtons();
    }

    private void UpdateInstruction()
    {
        if (assemblyInstruction == null)
            return;

        if (currentStep == 1)
        {
            assemblyInstruction.text =
                "Step 1: Place the bottom panel.";
        }
        else if (currentStep == 2)
        {
            assemblyInstruction.text =
                "Step 2: Attach the first side panel.";
        }
        else if (currentStep == 3)
        {
            assemblyInstruction.text =
                "Step 3: Attach the second side panel.";
        }
    }

    private void SetupButtons()
    {
        if (assemblyButton1 != null)
        {
            assemblyButton1.onClick.RemoveAllListeners();
            assemblyButton1.onClick.AddListener(
                () => SelectPanel(1)
            );
        }

        if (assemblyButton2 != null)
        {
            assemblyButton2.onClick.RemoveAllListeners();
            assemblyButton2.onClick.AddListener(
                () => SelectPanel(2)
            );
        }

        if (assemblyButton3 != null)
        {
            assemblyButton3.onClick.RemoveAllListeners();
            assemblyButton3.onClick.AddListener(
                () => SelectPanel(3)
            );
        }

        if (assemblyContinueButton != null)
        {
            assemblyContinueButton.onClick.RemoveAllListeners();
            assemblyContinueButton.onClick.AddListener(
                CloseAssembly
            );
        }
    }

    private void SelectPanel(int panelNumber)
    {
        if (currentStep == 1 && panelNumber == 1)
        {
            currentStep++;
            UpdateInstruction();
        }
        else if (currentStep == 2 &&
                 (panelNumber == 2 || panelNumber == 3))
        {
            currentStep++;
            UpdateInstruction();
        }
        else if (currentStep == 3 &&
                 (panelNumber == 2 || panelNumber == 3))
        {
            CompleteAssembly();
        }
    }

    private void CompleteAssembly()
    {
        if (progressionSystem != null)
        {
            progressionSystem.CompleteWorkshopAssembly();
        }

        if (finishedCrate != null)
        {
            finishedCrate.SetActive(true);
        }

        if (objectiveSystem != null)
        {
            objectiveSystem.SetObjective(completedObjective);
        }

        if (assemblyInstruction != null)
        {
            assemblyInstruction.text =
                "The crate has been assembled successfully.";
        }

        if (assemblyContinueButton != null)
        {
            assemblyContinueButton.gameObject.SetActive(true);
        }

        Debug.Log("Workshop crate assembly completed.");
    }

    public void CloseAssembly()
    {
        if (assemblyPanel != null)
        {
            assemblyPanel.SetActive(false);
        }

        if (interactionSystem != null)
        {
            interactionSystem.CloseWorkshopAssembly();
        }

        Debug.Log("Workshop assembly panel closed.");
    }
}