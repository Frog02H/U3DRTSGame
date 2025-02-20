using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Octrees
{
    public class Mover : MonoBehaviour
    {
        float speed = 5f;
        float accuracy = 1f;
        float turnSpeed = 5f;

        int currentWaypoint;
        OctreeNode currentNode;
        Vector3 destination;

        public OctreeGenerator octreeGenerator;
        Graph graph;

        void Start()
        {
            graph = octreeGenerator.waypoints;
            currentNode = GetClosestNode(transform.position);
            GetRandomDestination();
        }

        void Update()
        {
            if(graph == null){ return; }

            if(graph.GetPathLength() == 0 || currentWaypoint >= graph.GetPathLength())
            {
                GetRandomDestination();
                return;
            }

            if(Vector3.Distance(graph.GetPathNode(currentWaypoint).bounds.center, transform.position) < accuracy)
            {
                currentWaypoint++;
                Debug.Log($"Waypoint {currentWaypoint} reached");
            }

            if(currentWaypoint < graph.GetPathLength())
            {
                currentNode = graph.GetPathNode(currentWaypoint);
                destination = currentNode.bounds.center;

                Vector3 direction = destination - transform.position;
                direction.Normalize();

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else 
            {
                GetRandomDestination();
            }
        }

        OctreeNode GetClosestNode(Vector3 position)
        {
            return octreeGenerator.ot.FindClosestNode(transform.position);

            /* 
            //
            OctreeNode closestNode = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach(var nodePair in graph.nodes)
            {
                OctreeNode node = nodePair.Key;
                float distanceSqr = (node.bounds.center - position).sqrMagnitude;

                if(distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestNode = node;
                }
            }
            return closestNode; 
            */
        }

        void GetRandomDestination()
        {
            OctreeNode destinationNode;
            do
            {
                destinationNode = graph.nodes.ElementAt(Random.Range(0, graph.nodes.Count)).Key;
            } while(!graph.AStar(currentNode,destinationNode));

            currentWaypoint = 0;
        }

        //注下，绘制物体寻路的路径，包括遍历的点
        void OnDrawGizmos()
        {
            if(graph == null || graph.GetPathLength() == 0){return;}

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(graph.GetPathNode(8).bounds.center, 0.7f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(graph.GetPathNode(graph.GetPathLength() - 1).bounds.center, 0.7f);

            Gizmos.color = Color.green;
            for(int i = 0; i < graph.GetPathLength(); i++)
            {
                Gizmos.DrawWireSphere(graph.GetPathNode(i).bounds.center, 0.5f);
                if(i < graph.GetPathLength() - 1 )
                {
                    Vector3 start = graph.GetPathNode(i).bounds.center;
                    Vector3 end = graph.GetPathNode(i + 1).bounds.center;
                    Gizmos.DrawLine(start, end);
                }
            }
        }
    }

}
