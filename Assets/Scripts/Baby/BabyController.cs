using Unity.VisualScripting;
using UnityEngine;

namespace Baby
{
    public class BabyController : MonoBehaviour
    {
        private float moveTimer;
        private float laserTimer;
        [SerializeField] private float chillTime = 5f;
        [SerializeField] private float shootRate = 3f;
        
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private GameObject shardPrefab;
        private BabyMovementManager movement;
        [SerializeField] private MovementNode currentNode;
        private MovementNode nextGoal;

        void Start()
        {
            movement = GetComponent<BabyMovementManager>();
            nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
            while (nextGoal == currentNode)
            {
                nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
            }
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
                if (laserTimer >= shootRate)
                {
                    Quaternion angle = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    GameObject laser = Instantiate(laserPrefab, transform.position, angle);
                    LaserProjectile proj = laser.GetComponent<LaserProjectile>();
                    proj.moveDir = laser.transform.right;
                    laserTimer = 0;
                }
            }
        }

        public void SetGoalNode(Vector3 pos)
        {
            nextGoal = movement.GetNearestGoalNode(pos);
        }
    }
}
