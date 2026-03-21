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
                timer += Time.deltaTime;
                if (timer >= chillTime)
                {
                    movement.MoveTo(currentNode, nextGoal);
                    movement.isMoving = true;
                    currentNode = nextGoal;
                    nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
                    while (nextGoal == currentNode)
                    {
                        nextGoal = movement.goalNodes[Random.Range(0, movement.goalNodes.Count)];
                    }
                    timer = 0;
                }
            }
        }

        public void SetGoalNode(Vector3 pos)
        {
            nextGoal = movement.GetNearestGoalNode(pos);
        }
    }
}
