using UnityEngine;
using System.Collections;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject targetObject2;
    public float interval = 10f;

    public Player player;

    void Start()
    {
        StartCoroutine(ToggleRoutine());
    }

    IEnumerator ToggleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            player.darkToggle();

            if (targetObject != null)
            {
                targetObject.SetActive(!targetObject.activeSelf);
                targetObject2.SetActive(!targetObject.activeSelf);
            }
        }
    }
}