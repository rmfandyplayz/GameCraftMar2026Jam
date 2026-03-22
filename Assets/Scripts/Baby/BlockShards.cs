using UnityEngine;

public class BlockShards : DamagingObject
{
    [SerializeField] private float moveSpeed;
    [HideInInspector] public Vector2 moveDir;
    private float timer;
    [SerializeField] private float shardsStopMovingTime = .1f;
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < shardsStopMovingTime)
        {
            transform.position -= (Vector3)(moveDir.normalized * moveSpeed);
        }

        if (timer > 10f)
        {
            Destroy(this.gameObject);
        }
    }
}
