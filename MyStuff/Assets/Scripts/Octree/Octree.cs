using System;
using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{

    public class Octree
    {
        public OctreeNode root;
        public Bounds bounds;
        public Graph graph;
        List<OctreeNode> emptyLeaves = new();

        public Octree(GameObject[] worldObjects, float minNodeSize, Graph graph)
        {
            this.graph = graph;

            CalculateBounds(worldObjects);
            CreateTree(worldObjects, minNodeSize);

            GetEmptyLeaves(root);
            GetEdges();
            Debug.Log(graph.edges.Count);
        }

        //注下，从root和position开始遍历,position是物体当前的位置(transform.position),通过遍历root下的八叉树,递归查找出最接近的点
        public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(root, position);

        public OctreeNode FindClosestNode(OctreeNode node, Vector3 position) {
            OctreeNode found = null;
            for (int i = 0; i < node.children.Length; i++) {
                if (node.children[i].bounds.Contains(position)) {
                    if (node.children[i].IsLeaf) {
                        found = node.children[i];
                        break;
                    }
                    found = FindClosestNode(node.children[i], position);
                }
            }
            return found;
        }
        //

        private void GetEdges()
        {
            foreach(OctreeNode leaf in emptyLeaves)
            {
                foreach(OctreeNode otherLeaf in emptyLeaves)
                {
                    if(leaf.bounds.Intersects(otherLeaf.bounds))
                    {
                        graph.AddEdge(leaf, otherLeaf);
                    }
                }
            }
        }

        private void GetEmptyLeaves(OctreeNode node)
        {
            if(node.IsLeaf && node.objects.Count == 0)
            {
                emptyLeaves.Add(node);
                graph.AddNode(node);
                return;
            }

            if(node.children == null)
            {
                return;
            }

            foreach(OctreeNode child in node.children)
            {
                GetEmptyLeaves(child);
            }

            for(int i = 0; i < node.children.Length; i++)
            {
                for(int j = i + 1; j < node.children.Length; j++)
                {
                    graph.AddEdge(node.children[i], node.children[j]);
                }
            }
        }

        void CreateTree(GameObject[] worldObjects, float minNodeSize)
        {
            root = new OctreeNode(bounds, minNodeSize);

            foreach(var obj in worldObjects)
            {
                root.Divide(obj);
            }
        }

        //注下，统计GameObject数组,此为世界下的collider绘制包围盒bounds
        void CalculateBounds(GameObject[] worldObjects)
        {
            foreach (var obj in worldObjects) 
            {
                bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
                //将Encapsulate函数内遍历的bounds扩展
            }

            Vector3 size = Vector3.one * Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * 0.6f;
            bounds.SetMinMax(bounds.center - size, bounds.center + size);
        }
    }

}