
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BabyMindController : MonoBehaviour
{
    private Player player;
    private PlayerController controller;
    private float timer;

    private void OnEnable()
    {
        controller = FindAnyObjectByType<PlayerController>();
        controller.enabled = false;
    }

    private void OnDisable()
    {
        controller.enabled = true;
    }

    private void Update()
    {
        if (!player)
            player = FindAnyObjectByType<Player>();

        timer -= Time.deltaTime;
        if (!(timer <= 0)) return;
        
        timer = 0.5f;
        int rand = Random.Range(0, 3);
        player.move = rand switch
        {
            0 => Vector2.right,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.up,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
