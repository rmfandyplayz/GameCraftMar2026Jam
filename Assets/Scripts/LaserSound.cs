using UnityEngine;

public class LaserSound : MonoBehaviour
{
    public AudioSource audioSource;

    public bool hasSoundPlayed = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasSoundPlayed == false)
            {
                audioSource.Play();
                hasSoundPlayed = true;
            }
        }
    }
}
