
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BabyMindController : MonoBehaviour
{
    private Player player;
    private float timer;

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
        player.canMove = false;
        player.move = Vector2.zero;
    }

    private void OnDisable()
    {
        player.canMove = true;
    }

    private void Update()
    {
        if (!player)
            player = FindAnyObjectByType<Player>();

        timer -= Time.deltaTime;
        if (!(timer <= 0)) return;
        
        timer = 0.5f;
        int rand = Random.Range(0, 4);
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
