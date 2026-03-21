using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Image backgroundSprite;
    [SerializeField] private Image itemSprite;

    private Player playerRef;
    
    private void Awake()
    {
        playerRef = FindFirstObjectByType<Player>().GetComponent<Player>();
    }

    private void OnEnable()
    {
        if (playerRef != null)
        {
            playerRef.OnPickupItem.AddListener(PickupItem);
            playerRef.OnDropItem.AddListener(DropItem);
        }
    }

    private void OnDisable()
    {
        if (playerRef != null)
        {
            playerRef.OnPickupItem.RemoveListener(PickupItem);
            playerRef.OnDropItem.RemoveListener(DropItem);
        }
    }

    private void PickupItem(Sprite itemSprite)
    {
        
    }

    private void DropItem()
    {
        
    }
}
