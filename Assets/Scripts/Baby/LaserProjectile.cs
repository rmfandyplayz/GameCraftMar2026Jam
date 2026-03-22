using System;
using UnityEngine;

public class LaserProjectile : DamagingObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float audioRadius = 4f;

    [NonSerialized] public Vector2 moveDir;

    private Player player;
    public AudioSource audioSource;

    private float timer;
    private int bounceCount;

    private bool hasSoundPlayed = false;

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
        if (other.collider.CompareTag("Player"))
        {
            player.TakeDamage();
            Destroy(this.gameObject);
        }

        if (other.collider.CompareTag("Coffee"))
        {
            player.TakeDamage();
            Destroy(this.gameObject);
        }

        if (other.collider.CompareTag("Wall"))
        {
            Vector2 normal = other.contacts[0].normal;
            moveDir = Vector2.Reflect(moveDir, normal);

            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            bounceCount++;
        }
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