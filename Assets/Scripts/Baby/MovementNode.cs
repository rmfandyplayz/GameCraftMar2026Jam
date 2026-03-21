using System.Collections.Generic;
using UnityEngine;
namespace Baby
{
    public class MovementNode : MonoBehaviour
    { 
        [SerializeField] private List<MovementNode> adjacentNodes;
        public bool isGoalNode;
        public NodeLocation location;

        public List<MovementNode> GetAdjacentNodes()
        {
            return adjacentNodes;
        }
    }

    public enum NodeLocation
    {
        Kitchen, PlayRoom, Bedroom, None
    }
}


