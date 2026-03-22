using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Baby
{
    public class BabyMovementManager : MonoBehaviour
    {
        private MovementNode[] nodes;
        [HideInInspector] public List<MovementNode> goalNodes;
        private List<Connection> connections;
        [SerializeField] private float moveSpeed;
        [HideInInspector] public bool isMoving;
        [SerializeField] private bool canFly;
        private Animator anim;
        
        void Awake()
        {
            goalNodes = new();
            connections = new();
            anim = GetComponent<Animator>();
            
            nodes = FindObjectsByType<MovementNode>(FindObjectsSortMode.None);
            foreach (MovementNode node1 in nodes)
            {
                if (node1.isGoalNode)
                {
                    goalNodes.Add(node1);
                }
                foreach (MovementNode node2 in node1.GetAdjacentNodes())
                {
                    if (!connections.Contains(new Connection(node1, node2)))
                    {
                        connections.Add(new Connection(node1, node2));
                    }
                }
            }
        }

        public MovementNode GetNearestGoalNode(Vector3 pos)
        {
            MovementNode nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (MovementNode node in goalNodes)
            {
                if (Vector3.Distance(node.transform.position, pos) < nearestDistance)
                {
                    nearest = node;
                    nearestDistance = Vector3.Distance(node.transform.position, pos);
                }
            }

            return nearest;
        }

        public void MoveTo(MovementNode curr, MovementNode goal)
        {
            Dictionary<MovementNode, float> distances = new();
            PriorityQueue<MovementNode> pq = new();
            Dictionary<MovementNode, MovementNode> previous = new();

            foreach (MovementNode node in nodes)
            {
                distances.Add(node, float.PositiveInfinity);
                previous.Add(node, null);
            }

            distances[curr] = 0;
            pq.Enqueue(curr, distances[curr]);
            while (pq.Count > 0)
            {
                (MovementNode, float) queueItem = pq.Dequeue();
                MovementNode currentNode = queueItem.Item1;
                float currentDistance = queueItem.Item2;

                if (currentNode == goal)
                {
                    break;
                }

                if (!(currentDistance > distances[currentNode]))
                {
                    foreach (MovementNode adjacentNode in currentNode.GetAdjacentNodes())
                    {
                        if (distances[currentNode] + Vector3.Distance(currentNode.transform.position,
                                adjacentNode.transform.position) < distances[adjacentNode])
                        {
                            distances[adjacentNode] = distances[currentNode] +
                                                      Vector3.Distance(currentNode.transform.position,
                                                          adjacentNode.transform.position);
                            previous[adjacentNode] = currentNode;
                            pq.Enqueue(adjacentNode, distances[adjacentNode]);
                        }
                    }
                }

            }

            List<MovementNode> path = new();
            MovementNode pathNode = goal;
            while (pathNode != null)
            {
                path.Insert(0, pathNode); // Insert at start to reverse the path
                pathNode = previous[pathNode];
            }
            isMoving = true;
            StartCoroutine(TravelPath(path));
        }

        private IEnumerator TravelPath(List<MovementNode> path)
        {
            if (canFly)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isFlying", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isPlaying", false);
                anim.SetBool("isNapping", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
                anim.SetBool("isFlying", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isPlaying", false);
                anim.SetBool("isNapping", false);
            }

            foreach (MovementNode node in path)
            {
                Vector3 targetPosition = node.transform.position;
                Vector3 travelVector = targetPosition - transform.position;
                if (Math.Abs(travelVector.x) >= Math.Abs(travelVector.y))
                {
                    anim.SetFloat("directionIndex", 1f);
                    GetComponent<SpriteRenderer>().flipX = travelVector.x > 0;
                }
                else
                {
                    if (travelVector.y > 0)
                    {
                        anim.SetFloat("directionIndex", 2f);
                    }
                    else
                    {
                        anim.SetFloat("directionIndex", 0f);
                    }
                }

                // Move towards this node until we reach it
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        targetPosition,
                        moveSpeed * Time.deltaTime
                    );

                    yield return null; // wait until next frame
                }

                transform.position = targetPosition;
            }
            isMoving = false;
            anim.SetBool("isMoving" , false);
            if (path[^1].location == NodeLocation.PlayRoom)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isFlying", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isPlaying", true);
                anim.SetBool("isNapping", false);
            }
            else if (path[^1].location == NodeLocation.Bedroom)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isFlying", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isPlaying", false);
                anim.SetBool("isNapping", true);
            }
            else if (path[^1].location == NodeLocation.Kitchen)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isFlying", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isPlaying", false);
                anim.SetBool("isNapping", false);
                anim.SetFloat("directionIndex", 2f);
            }
            else
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isFlying", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isPlaying", false);
                anim.SetBool("isNapping", false);
                anim.SetFloat("directionIndex", 0f);
            }
        }



        public struct Connection
        {
            public MovementNode node1;
            public MovementNode node2;

            public Connection(MovementNode node1, MovementNode node2)
            {
                this.node1 = node1;
                this.node2 = node2;
            }

            public bool Equals(Connection connection)
            {
                if (((node1 != connection.node1) && (node1 != connection.node2)) ||
                    ((node2 != connection.node1) && (node2 != connection.node2)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class PriorityQueue<T>
        {
            private List<(T item, float priority)> _heap = new();

            public int Count => _heap.Count;

            // Add element
            public void Enqueue(T item, float priority)
            {
                _heap.Add((item, priority));
                HeapifyUp(_heap.Count - 1);
            }

            // Remove smallest priority
            public (T, float) Dequeue()
            {
                if (_heap.Count == 0)
                    throw new InvalidOperationException("Queue is empty");

                (T, float) root = _heap[0];

                _heap[0] = _heap[^1];
                _heap.RemoveAt(_heap.Count - 1);

                HeapifyDown(0);

                return root;
            }

            // Heap helpers
            private void HeapifyUp(int index)
            {
                while (index > 0)
                {
                    int parent = (index - 1) / 2;

                    if (_heap[index].priority >= _heap[parent].priority)
                        break;

                    (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
                    index = parent;
                }
            }

            private void HeapifyDown(int index)
            {
                int lastIndex = _heap.Count - 1;

                while (true)
                {
                    int left = 2 * index + 1;
                    int right = 2 * index + 2;
                    int smallest = index;

                    if (left <= lastIndex && _heap[left].priority < _heap[smallest].priority)
                        smallest = left;

                    if (right <= lastIndex && _heap[right].priority < _heap[smallest].priority)
                        smallest = right;

                    if (smallest == index)
                        break;

                    (_heap[index], _heap[smallest]) = (_heap[smallest], _heap[index]);
                    index = smallest;
                }
            }
        }
    }
}
