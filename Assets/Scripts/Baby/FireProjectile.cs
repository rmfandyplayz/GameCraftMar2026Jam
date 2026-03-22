using System;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float growSpeed;
    
    private float timer;
    
    [SerializeField] private float stopMovingTime = .3f;
    [SerializeField] private float growTime = 3f;
    
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < stopMovingTime)
        {
            transform.position += (Vector3)(moveDir.normalized * moveSpeed);
        }

        if (timer < growTime)
        {
            transform.localScale += new Vector3(growSpeed, growSpeed, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.TakeDamage();
        }

        if (other.tag == "Wall")
        {
            growTime = timer;
        }

        if (other.tag == "Ice")
        {
            Destroy(this.gameObject);
        }
}
}
