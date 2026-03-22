using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Settings")]
    public bool sceneTrans = true;

    [Header("Transition")]
    public Animator transitionAnimator; 
    public float transitionDelay = 1f;

    private bool isLoading = false;

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        if (sceneTrans && transitionAnimator)
        {
            transitionAnimator.SetTrigger("end");
            yield return new WaitForSeconds(transitionDelay);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
