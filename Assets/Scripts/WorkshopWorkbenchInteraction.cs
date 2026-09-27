using UnityEngine;

public class WorkshopWorkbenchInteraction : MonoBehaviour
{
    [Header("Workbench Information")]
    [SerializeField] private string workbenchName = "Workshop Workbench";

    [SerializeField]
    [TextArea(2, 4)]
    private string interactionMessage =
        "You can assemble the crate here.";

    [Header("Assembly")]
    [SerializeField] private WorkshopAssemblySystem assemblySystem;

    public string WorkbenchName => workbenchName;
    public string InteractionMessage => interactionMessage;
    public WorkshopAssemblySystem AssemblySystem => assemblySystem;
}