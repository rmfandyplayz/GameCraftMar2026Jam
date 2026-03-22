using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Baby
{
    public class BabyController : MonoBehaviour
    {
        private float moveTimer;
        private float laserTimer;
        private float mcTimer;
        
        [SerializeField] private float chillTime = 5f;
        [SerializeField] private float shootRate = 3f;
        [SerializeField] private float mindControlRate = 4f;
        [SerializeField] private float mcRadius = 3f;
        [SerializeField] private float mcDuration = 3f;
        
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private GameObject shardPrefab;
        [SerializeField] private GameObject firePrefab;
        
        private GameObject player;
        
        private BabyMovementManager movement;
        private BabyMindController mindControl;
        [SerializeField] private MovementNode currentNode;
        private MovementNode nextGoal;

        void Start()
        {
            movement = GetComponent<BabyMovementManager>();
            mindControl = GetComponent<BabyMindController>();
            mindControl.enabled = false;
            nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
            while (nextGoal == currentNode)
            {
                nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
            }

            //Finds the player
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            if (!movement.isMoving)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer >= chillTime)
                {
                    movement.MoveTo(currentNode, nextGoal);
                    if (currentNode.location == NodeLocation.PlayRoom)
                    {
                        for (int i = 0; i <= 360; i += 30)
                        {
                            Quaternion angle = Quaternion.Euler(0, 0, i);
                            GameObject shard = Instantiate(shardPrefab, transform.position, angle);
                            BlockShards proj = shard.GetComponent<BlockShards>();
                            proj.moveDir = shard.transform.up;
                        }
                    }
                    
                    if (currentNode.location == NodeLocation.Kitchen)
                    {
                        Quaternion angle = Quaternion.Euler(0, 0, 0);
                        GameObject fire = Instantiate(firePrefab, transform.position, angle);
                        FireProjectile proj = fire.GetComponent<FireProjectile>();
                        proj.moveDir = fire.transform.up;
                    }

                    movement.isMoving = true;
                    currentNode = nextGoal;
                    nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
                    while (nextGoal == currentNode)
                    {
                        nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
                    }
                    moveTimer = 0;
                }
            }
            else
            {
                laserTimer +=  Time.deltaTime;
                mcTimer += Time.deltaTime;

                if (laserTimer >= shootRate)
                {
                    if (player != null)
                    {
                        //Lasers aim at player
                        Vector2 direction = (player.transform.position - transform.position).normalized;

                        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);

                        GameObject laser = Instantiate(laserPrefab, transform.position, rotation);

                        LaserProjectile proj = laser.GetComponent<LaserProjectile>();
                        proj.moveDir = direction;
                    }

                    laserTimer = 0;
                }

                if (mcTimer >= mindControlRate)
                {
                    mcTimer = 0;
                    foreach (Collider2D hit in Physics2D.OverlapCircleAll(transform.position, mcRadius))
                    {
                        if (hit.tag == "Player")
                        {
                            mindControl.enabled = true;
                            StartCoroutine(TimeMindControl()); }
                    }
                }
            }
        }

        private IEnumerator TimeMindControl()
        {
            yield return new WaitForSeconds(mcDuration);
            mindControl.enabled = false;
        }

        public void SetGoalNode(Vector3 pos)
        {
            nextGoal = movement.GetNearestGoalNode(pos);
        }
    }
}
