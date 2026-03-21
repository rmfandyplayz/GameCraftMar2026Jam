using System;
using UnityEngine;
using System.Collections.Generic;

// manager for all the health icons, listens to player health event updates
public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private List<HeartUI> hearts;

    private int previousHP;
    private Player playerRef;

    private void Awake()
    {
        playerRef = FindFirstObjectByType<Player>().GetComponent<Player>();
    }

    private void OnEnable()
    {
        if (playerRef != null)
            playerRef.OnHPChanged.AddListener(UpdateHearts);
    }

    private void OnDisable()
    {
        if (playerRef != null)
            playerRef.OnHPChanged.RemoveListener(UpdateHearts);
    }

    // call this from ur player or game manager when the level starts
    public void InitHearts(int currentHP, int maxHP)
    {
        SetMaxHearts(maxHP);
        previousHP = currentHP;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (!hearts[i].gameObject.activeSelf) continue;
            
            // uses ur silent init method
            hearts[i].SetState(i < currentHP);
        }
    }
    
    public void SetMaxHearts(int maxHP)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < maxHP);
        }
    }

    private void UpdateHearts(int currentHP)
    {
        if (currentHP == previousHP) return;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (!hearts[i].gameObject.activeSelf) continue;

            bool shouldBeFull = i < currentHP;
            bool wasFull = i < previousHP;

            // only play animations on the hearts that actually changed state
            if (shouldBeFull && !wasFull)
            {
                hearts[i].Heal();
            }
            else if (!shouldBeFull && wasFull)
            {
                hearts[i].Damage();
            }
        }

        // cache it for the next time player takes damage
        previousHP = currentHP;
    }
}