using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    public UnityEvent<string> OnObjectiveUpdate = new();
    public string objectiveText = "Objective Here";
    
    void Start()
    {
        OnObjectiveUpdate.Invoke(objectiveText);
    }
}
