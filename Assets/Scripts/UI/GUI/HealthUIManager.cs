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

    // call this during start or smth
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
        RefreshBeatingHeart(currentHP);
    }

    // TODO: delete when done
    public void InitHPTEST()
    {
        InitHearts(3, 3);
    }
    
    public void SetMaxHearts(int maxHP)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < maxHP);
        }
    }

    public void UpdateHearts(int currentHP)
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
        RefreshBeatingHeart(currentHP);
    }
    
    private void RefreshBeatingHeart(int currentHP)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (!hearts[i].gameObject.activeSelf) 
                continue;

            // the rightmost full heart is at index hp - 1
            // e.g. if u have 3 hp, index 2 is the rightmost. 
            bool isRightmost = (i == currentHP - 1);
        
            hearts[i].SetBeating(isRightmost); 
        }
    }
}