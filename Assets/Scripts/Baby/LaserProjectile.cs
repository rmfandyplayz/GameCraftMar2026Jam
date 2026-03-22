
using System;
using UnityEngine;

public class LaserProjectile : DamagingObject
{
    [SerializeField] private float moveSpeed;
    [NonSerialized] public Vector2 moveDir;
    private Player player;
    private float timer;
    private int bounceCount;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        transform.position += (Vector3)(moveDir.normalized * moveSpeed);
        
        if (bounceCount > 3)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            player.TakeDamage();
            Destroy(this.gameObject);
        }

        if (other.collider.tag == "Wall")
        {
            Vector2 normal = other.contacts[0].normal;
            moveDir = Vector2.Reflect(moveDir, normal);
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            bounceCount++;
        }
    }
}
