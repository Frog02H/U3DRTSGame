using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Octrees 
{
    public class OctreeGenerator : MonoBehaviour
    {
        public GameObject[] objects;
        public float minNodeSize = 1f;
        public Octree ot;

        public readonly Graph waypoints = new();

        void Awake() => ot = new Octree(objects, minNodeSize, waypoints);

        void OnDrawGizmos()
        {
            if(!Application.isPlaying)
            {
                return;
            }
            
            Gizmos.color = Color.green;
            //Gizmos.DrawWireCube(ot.bounds.center, ot.bounds.size);
            //bounds范围图像绘制

            ot.root.DrawNode();
            //ot.graph.DrawGraph();
            //bounds中心连线范围图像绘制
        }
    }
    
}
