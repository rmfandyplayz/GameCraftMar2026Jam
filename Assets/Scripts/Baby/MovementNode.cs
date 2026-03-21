using System.Collections.Generic;
using UnityEngine;
namespace Baby
{
    public class MovementNode : MonoBehaviour
    { 
        [SerializeField] private List<MovementNode> adjacentNodes;

        public List<MovementNode> GetAdjacentNodes()
        {
            return adjacentNodes;
        }
    }
}
