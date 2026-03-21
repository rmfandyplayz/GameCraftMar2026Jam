using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

// written by andy
// handles multi-item transitions in the main menu
public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private CanvasGroup buttonGroup;

    private bool lockAllOperations; // disallow all buttons from being interacted with

    public void StartGame()
    {
        
    }

    public void OpenSettings()
    {
        
    }
    
    public void OpenCredits()
    {
        
    }

    /// <summary>
    /// Sets the "interactable" state of ALL buttons in this scene to either true or false
    /// </summary>
    /// <param name="newState">new state to set all buttons to</param>
    public void SetAllButtonEnabledState(bool newState)
    {
        buttonGroup.enabled = newState;
    }
}
