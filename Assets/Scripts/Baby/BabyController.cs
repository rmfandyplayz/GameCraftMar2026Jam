using UnityEngine;

namespace Baby
{
    public class BabyController : MonoBehaviour
    {
        private float timer;
        [SerializeField] private float chillTime = 5f;
        private BabyMovementManager movement;
        [SerializeField] private MovementNode currentNode;
        private MovementNode nextGoal;

        void Start()
        {
            movement = GetComponent<BabyMovementManager>();
            nextGoal = movement.nodes[Random.Range(0, movement.nodes.Length)];
            while (nextGoal == currentNode)
            {
                nextGoal = movement.nodes[Random.Range(0, movement.nodes.Length)];
            }
        }

        void Update()
        {
            if (!movement.isMoving)
            {
                timer += Time.deltaTime;
                if (timer >= chillTime)
                {
                    movement.MoveTo(currentNode, nextGoal);
                    movement.isMoving = true;
                    currentNode = nextGoal;
                    nextGoal = movement.nodes[Random.Range(0, movement.nodes.Length)];
                    while (nextGoal == currentNode)
                    {
                        nextGoal = movement.nodes[Random.Range(0, movement.nodes.Length)];
                    }
                    timer = 0;
                }
            }
        }

        public void SetGoalNode(Vector3 pos)
        {
            nextGoal = movement.GetNearestNode(pos);
        }
    }
}
