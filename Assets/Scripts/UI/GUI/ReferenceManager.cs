using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// listens to input & events and controls the visibility of stuff like
// the quests tab and the minimap
public class ReferenceManager : MonoBehaviour
{
    public static bool lockAllInteractions; // prevent any key from doing anything
    
    [SerializeField] private MapUI mapUI;
    [SerializeField] private ObjectivesUI objectivesUI;

    private Player playerRef;
    
    private void Awake()
    {
        playerRef = FindFirstObjectByType<Player>().GetComponent<Player>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void EnableObjectives()
    {
        if (lockAllInteractions)
            return;
        
        objectivesUI.Open();
        mapUI.HideIcon();
    }

    public void EnableMap()
    {
        if (lockAllInteractions)
            return;
        
        mapUI.Open();
        objectivesUI.HideIcon();
    }

    public void DisableObjectives()
    {
        if (lockAllInteractions)
            return;
        
        objectivesUI.Close();
        mapUI.ShowIcon();
    }
    
    public void DisableMap()
    {
        if (lockAllInteractions)
            return;
        
        mapUI.Close();
        objectivesUI.ShowIcon();
    }
}
