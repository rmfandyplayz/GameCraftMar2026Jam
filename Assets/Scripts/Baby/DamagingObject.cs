
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DamagingObject : MonoBehaviour
{

    protected virtual void OnHit()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            var player = other.GetComponent<Player>();
            player.TakeDamage();
        }
        OnHit();
    }
}
