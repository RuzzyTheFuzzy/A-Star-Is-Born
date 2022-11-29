using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{

    public static List<Vector3> FindPath( int nodeLimit,Vector2Int startPos , Vector2Int goalPos )
    {
        var start = new Node
        {
            position = startPos
        };

        var goal = new Node
        {
            position = goalPos
        };

        start.SetDistance( goal.position );

        var activeNodes = new List<Node> { start };
        var visitedNodes = new List<Node>();

        while ( activeNodes.Any() )
        {
            if ( visitedNodes.Count > nodeLimit )
            {
                break;
            }

            var checkNode = activeNodes.OrderBy( x => x.CostDist ).First();

            if ( checkNode.position == goal.position )
            {
                var node = checkNode;

                var path = new List<Vector3>();

                while ( true )
                {
                    path.Add( ( Vector2 )node.position );
                    node = node.parent;

                    if ( node == null )
                    {
                        path.Reverse();
                        Debug.Log( "Visited " + visitedNodes.Count + " Nodes before finding the goal");
                        return path;
                    }
                }
            }

            visitedNodes.Add( checkNode );
            activeNodes.Remove( checkNode );

            var possibleNodes = GetPossibleNodes( checkNode, goal );
            // Debug.Log(possibleNodes[0]);

            foreach ( var node in possibleNodes )
            {
                // We have visited it before, so skip
                if ( visitedNodes.Any( x => x.position == node.position ) )
                {
                    continue;
                }

                // We have seen it before, is this a better path to the node?
                if ( activeNodes.Any( x => x.position == node.position ) )
                {
                    var existingNode = activeNodes.First( x => x.position == node.position );

                    if ( existingNode.CostDist > node.CostDist )
                    {
                        activeNodes.Remove( existingNode );
                        activeNodes.Add( ( node ) );
                    }
                }
                else
                {
                    activeNodes.Add( node );
                }
            }
        }

        Debug.Log( "No Path Found :'(" );
        return new List<Vector3>();
    }

    private static List<Node> GetPossibleNodes( Node currentNode, Node goalNode )
    {
        // Add all 8 of the neightbours
        var possibleNodes = new List<Node>()
        {
            new Node // Up
            {
                position = currentNode.position + Vector2Int.up, parent = currentNode, cost = currentNode.cost + 1
            },
            new Node // Down
            {
                position = currentNode.position + Vector2Int.down, parent = currentNode, cost = currentNode.cost + 1
            },
            new Node // Left
            {
                position = currentNode.position + Vector2Int.left, parent = currentNode, cost = currentNode.cost + 1
            },
            new Node // Right
            {
                position = currentNode.position + Vector2Int.right, parent = currentNode, cost = currentNode.cost + 1
            },
            new Node // Up Right
            {
                position = currentNode.position + Vector2Int.one, parent = currentNode, cost = currentNode.cost + 1
            },
            new Node // Down Left
            {
                position = currentNode.position - Vector2Int.one, parent = currentNode, cost = currentNode.cost + 1
            },
            new Node // Down Right
            {
                position = currentNode.position + new Vector2Int( 1, -1 ), parent = currentNode,
                cost = currentNode.cost + 1
            },
            new Node // Up Left
            {
                position = currentNode.position + new Vector2Int( -1, 1 ), parent = currentNode,
                cost = currentNode.cost + 1
            }
        };

        possibleNodes.ForEach( node => node.SetDistance( goalNode.position ) );

        return possibleNodes
            .Where( node =>
                !Physics2D.CircleCast( currentNode.position, 0.5f,
                    ( node.position - currentNode.position ),
                    Vector2.Distance( currentNode.position, node.position ) ) )
            .ToList();
    }

    public class Node
    {
        public Vector2Int position;

        public int cost;
        public float distance;
        public float CostDist => cost + distance;
        public Node parent;

        public void SetDistance( Vector2Int goal )
        {
            this.distance = Vector2Int.Distance( this.position, goal );
        }

        public override string ToString( )
        {
            return this.position.ToString();
        }
    }
}