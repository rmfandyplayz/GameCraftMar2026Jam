
using System;
using UnityEngine;

public class LaserProjectile : DamagingObject
{
    [SerializeField] private float moveSpeed;
    [NonSerialized] public Vector2 moveDir;
    [SerializeField] private float laserDestroyTime = 3f;
    private float timer;
    
    
    private void Update()
    {
        timer += Time.deltaTime;
        transform.position += (Vector3)(moveDir.normalized * moveSpeed);
        if (timer > laserDestroyTime)
        {
            Destroy(this.gameObject);
        }
    }
}
