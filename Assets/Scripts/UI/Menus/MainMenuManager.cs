using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// written by andy
// handles multi-item transitions in the main menu
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup buttonGroup;
    [SerializeField] private Image creditsGroup;

    private bool lockAllOperations; // disallow all buttons from being interacted with

    public void StartGame()
    {
        SceneManager.LoadScene("Comic1");
    }

    public void OpenSettings()
    {
        
    }
    
    public void OpenCredits()
    {
        creditsGroup.gameObject.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsGroup.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
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
