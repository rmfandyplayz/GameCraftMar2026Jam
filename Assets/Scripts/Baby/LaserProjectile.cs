
using System;
using UnityEngine;

public class LaserProjectile : DamagingObject
{
    [SerializeField] private float MoveSpeed;
    [NonSerialized] public Vector2 moveDir;
    
    
    private void Update()
    {
        transform.position += (Vector3)(moveDir.normalized * MoveSpeed);
    }
}
