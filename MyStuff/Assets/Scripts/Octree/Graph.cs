using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Octrees
{

    public class Node
    {
        static int nextId;
        //children节点的Index
        public readonly int id;
        //当前选中节点的Index
        public float f, g, h;
        //A* 算法 F = G + H
        public Node from;
        //该节点的父节点
        public List<Edge> edges = new();

        public OctreeNode octreeNode;
        //八叉树做一种静态的映射

        public Node(OctreeNode octreeNode)
        {
            this.id = nextId++;
            this.octreeNode = octreeNode;
        }

        public override bool Equals(object obj) => obj is Node other && id == other.id;
        public override int GetHashCode() => id.GetHashCode();

    }

    public class Edge
    {
        public readonly Node a, b;

        public Edge(Node a, Node b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Equals(object obj)
        {
            return obj is Edge other && ((a == other.a && b ==other.b) || (a == other.b && b == other.a));
        }

        public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();
        //GetHashCode函数一般是在操作HashTable或者Dictionary之类的数据集的时候被调用，目的是产生一个Key，为了方便在 HashTable或者 Dictionary中的检索。
    }

    public class Graph
    {
        public readonly Dictionary<OctreeNode, Node> nodes = new();
        public readonly HashSet<Edge> edges = new();

        List<Node> pathList = new();

        public int GetPathLength() => pathList.Count;

        public OctreeNode GetPathNode(int index)
        {
            if(pathList == null)
            {
                return null;
            }

            if(index < 0 || index >= pathList.Count)
            {
                Debug.LogError($"Index out of bounds. Path length: {pathList.Count}, Index: {index}");
                return null;
            }

            return pathList[index].octreeNode;
        }

        const int maxIterations = 10000;

        /* 
        F = G + H
        G = 从起点 A 移动到指定方格的移动代价，沿着到达该方格而生成的路径。
        H = 从指定的方格移动到终点 B 的估算成本，试探法。
        F = 实际花费。
        反复遍历 open list ，选择 F 值最小的方格。
        
        为了继续搜索，我们从 open list 中选择 F 值最小的 ( 方格 ) 节点，然后对所选择的方格作如下操作：
            
            把它从 open list 里取出，放到 close list 中。
            
            检查所有与它相邻的方格，忽略其中在 close list 中或是不可走 (unwalkable) 的方格 ( 比如墙，水，或是其他非法地形 ) ，如果方格不在open lsit 中，则把它们加入到 open list 中。
            
            把我们选定的方格设置为这些新加入的方格的父亲，新方格有个指针指向选定方格。（该Graph的public Node from的from就是父母节点）

            如果某个相邻的方格已经在 open list 中，则检查这条路径是否更优，也就是说经由当前方格 ( 我们选中的方格 ) 到达那个方格是否具有更小的 G 值。如果没有，不做任何操作。

            相反，如果 G 值更小，则把那个方格的父亲设为当前方格 ( 我们选中的方格 ) ，然后重新计算那个方格的 F 值和 G 值。如果你还是很混淆，请参考下图。
        */

        //注下,A*算法, 
        public bool AStar(OctreeNode startNode, OctreeNode endNode)
        {
            pathList.Clear();
            Node start = FindNode(startNode);
            Node end = FindNode(endNode);

            if(start == null && end == null)
            {
                Debug.LogError("Start Or End node are not found in the graph.");
                return false;
            }

            SortedSet<Node> openSet = new(new NodeComparer());
            HashSet<Node> closedSet = new();
            int iterationCount = 0;

            start.g = 0;
            start.h = Heuristic(start, end);
            start.f = start.g + start.h;
            start.from = null;
            openSet.Add(start);

            while(openSet.Count > 0)
            {
                if(++iterationCount > maxIterations)
                {
                    Debug.LogError("A* exceeded(超过) maximum iterations.");
                    return false;
                }

                Node current = openSet.First();
                openSet.Remove(current);

                if(current.Equals(end))
                {
                    ReconstructPath(current);
                    return true;
                }

                closedSet.Add(current);

                foreach(Edge edge in current.edges)
                {
                    Node neighbor = Equals(edge.a, current) ? edge.b : edge.a;

                    if(closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    float tentative_gScore = current.g + Heuristic(current, neighbor);

                    if(tentative_gScore < neighbor.g || !openSet.Contains(neighbor))
                    {
                        neighbor.g = tentative_gScore;
                        neighbor.h = Heuristic(neighbor, end);
                        neighbor.f = neighbor.g + neighbor.f;
                        neighbor.from = current;
                        openSet.Add(neighbor);
                    }
                }
            }

            Debug.Log("No path found.");
            return false;
        }

        void ReconstructPath(Node current)
        {
            while(current != null)
            {
                pathList.Add(current);
                current = current.from;
            }

            //因为openSet最后一个元素
            pathList.Reverse();
        }

        float Heuristic(Node a, Node b) => (a.octreeNode.bounds.center - b.octreeNode.bounds.center).sqrMagnitude;
        //sqrMagnitude计算平方数

        //通过实现下方的比较接口，得以使我们的开集保持排序，第一个走出的节点永远是消费最低的节点
        public class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                if(x == null || y == null)
                {
                    return 0;
                }

                int compare = x.f.CompareTo(y.f);

                if(compare == 0)
                {
                    return x.id.CompareTo(y.id);
                }

                return compare;
            }
        }

        public void AddNode(OctreeNode octreeNode)
        {
            if(!nodes.ContainsKey(octreeNode))
            {
                nodes.Add(octreeNode, new Node(octreeNode));
            }
        }

        public void AddEdge(OctreeNode a, OctreeNode b)
        {
            Node nodeA = FindNode(a);
            Node nodeB = FindNode(b);

            if(nodeA == null || nodeB == null)
            {
                return;
            }

            var edge = new Edge(nodeA, nodeB);
            
            if(edges.Add(edge))
            {
                nodeA.edges.Add(edge);
                nodeB.edges.Add(edge);
            }
        }

        Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out Node node);
            return node;
        }

        //注下，绘制出edge两octree端点的边界中心之间的线段
        public void DrawGraph()
        {
            Gizmos.color = Color.red;
            foreach(Edge edge in edges)
            {
                Gizmos.DrawLine(edge.a.octreeNode.bounds.center, edge.b.octreeNode.bounds.center);
            }

            foreach(var node in nodes.Values)
            {
                Gizmos.DrawWireSphere(node.octreeNode.bounds.center, 0.2f);
            }
        }

    }
}
