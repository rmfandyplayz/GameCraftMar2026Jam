using UnityEngine;

public class Coffee : MonoBehaviour
{
    public SceneTransition sceneTrans;
    public string SceneName;
    void Start()
    {
        sceneTrans = FindFirstObjectByType<SceneTransition>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (sceneTrans != null)
            {
                sceneTrans.LoadScene(SceneName);
            }
            else
            {
                Debug.LogWarning("SceneTransition not found!");
            }
        }
    }
}
