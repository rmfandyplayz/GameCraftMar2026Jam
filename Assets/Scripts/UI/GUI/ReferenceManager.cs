using UnityEngine;
using UnityEngine.InputSystem;

// listens to input & events and controls the visibility of stuff like
// the quests tab and the minimap
public class ReferenceManager : MonoBehaviour
{
    public static bool lockAllInteractions; // prevent any key from doing anything
    
    [SerializeField] private MapUI mapUI;
    [SerializeField] private ObjectivesUI objectivesUI;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference mapToggleAction;
    [SerializeField] private InputActionReference objectiveToggleAction;

    private Objective objectiveRef;
    private int opened; // 0 = nothing opened, 1 = map opened, 2 = objective opened
    
    private void Awake()
    {
        objectiveRef = FindFirstObjectByType<Objective>();
    }

    private void OnEnable()
    {
        if (objectiveRef != null)
            objectiveRef.OnObjectiveUpdate.AddListener(UpdateObjective);

        if (mapToggleAction != null)
        {
            mapToggleAction.action.Enable();
            mapToggleAction.action.performed += OnMapTogglePerformed;
        }

        if (objectiveToggleAction != null)
        {
            objectiveToggleAction.action.Enable();
            objectiveToggleAction.action.performed += OnObjectiveTogglePerformed;
        }
    }

    private void OnDisable()
    {
        if (objectiveRef != null)
            objectiveRef.OnObjectiveUpdate.RemoveListener(UpdateObjective);

        if (mapToggleAction != null)
            mapToggleAction.action.performed -= OnMapTogglePerformed;

        if (objectiveToggleAction != null)
            objectiveToggleAction.action.performed -= OnObjectiveTogglePerformed;
    }

    public void UpdateObjective(string newObjective)
    {
        objectivesUI.UpdateObjectiveText(newObjective);
    }

    private void OnMapTogglePerformed(InputAction.CallbackContext context)
    {
        if (opened == 1)
            DisableMap();
        else
            EnableMap();
    }

    private void OnObjectiveTogglePerformed(InputAction.CallbackContext context)
    {
        if (opened == 2)
            DisableObjectives();
        else
            EnableObjectives();
    }

    public void EnableObjectives()
    {
        if (lockAllInteractions || opened == 1)
            return;
        
        objectivesUI.Open();
        mapUI.HideIcon();
        opened = 2;
    }

    public void EnableMap()
    {
        if (lockAllInteractions || opened == 2)
            return;
        
        mapUI.Open();
        objectivesUI.HideIcon();
        opened = 1;
    }

    public void DisableObjectives()
    {
        if (lockAllInteractions || opened == 0 || opened == 1)
            return;
        
        objectivesUI.Close();
        mapUI.ShowIcon();
        opened = 0;
    }
    
    public void DisableMap()
    {
        if (lockAllInteractions || opened == 0 || opened == 2)
            return;
        
        mapUI.Close();
        objectivesUI.ShowIcon();
        opened = 0;
    }
}