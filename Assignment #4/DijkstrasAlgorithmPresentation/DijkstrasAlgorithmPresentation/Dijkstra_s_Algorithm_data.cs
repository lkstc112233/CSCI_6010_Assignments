using System.Collections.Generic;

namespace DijkstrasAlgorithmPresentation
{
    public class Dijkstra_s_Algorithm_data
    {
        private Heap<Vertex> heap = new Heap<Vertex>((Vertex a, Vertex b) => { return a.cost < b.cost; });
        private Graph graph;
        public Dijkstra_s_Algorithm_data(Graph g)
        {
            graph = g;
        }
        public void setStartPoint(Vertex v)
        {
            v.cost = 0;
            heap.Add(v);
        } 
        public void setEndPoint(Vertex v)
        {
            TargetVertex = v;
        }

        Vertex TargetVertex = null;

        Vertex currentVertex = null;
        Edge currentEdge = null;
        Stack<Edge> appendingEdges = new Stack<Edge>();
        public bool PathFound = false;

        public void ResetStatus()
        {
            TargetVertex = null;
            currentEdge = null;
            currentEdge = null;
            appendingEdges.Clear();
            while (!heap.IsEmpty())
                heap.RemoveMin();
        }

        private Edge TakeNextEdge()
        {
            if (appendingEdges.Count > 0)
                return appendingEdges.Pop();
            if (heap.IsEmpty())
                return null;
            var v = heap.GetMin();
            if (ReferenceEquals(v, TargetVertex))
            {
                PathFound = true;
                return null;
            }
            heap.RemoveMin();
            if (currentVertex != null)
                currentVertex.SetType(VertexType.ScannedVertex);
            currentVertex = v;
            currentVertex.SetType(VertexType.ScanningVertex);
            for (int i = 0; i < graph.EdgeTable.GetLength(0); ++i)
            {
                if (graph.EdgeTable[v.id, i] != null)
                {
                    appendingEdges.Push(graph.EdgeTable[v.id, i]);
                    graph.EdgeTable[v.id, i].SetType(EdgeType.ListedEdge);
                }
            }
            return TakeNextEdge();
        }

        public bool OneStep()
        {
            if (PathFound)
                return false;
            if (currentEdge != null)
                currentEdge.SetType(EdgeType.ScannedEdge);
            currentEdge = TakeNextEdge();
            currentEdge.SetType(EdgeType.ScanningEdge);
            if (currentEdge == null)
                return false;
            Vertex nextVertex;
            if (currentEdge.oneway)
            {
                nextVertex = currentEdge.end;
            }
            else
            {
                if (currentEdge.start == currentVertex)
                    nextVertex = currentEdge.end;
                else
                    nextVertex = currentEdge.start;
            }
            if (nextVertex.cost < 0)
            {
                nextVertex.cost = currentVertex.cost + currentEdge.weight;
                nextVertex.SetType(VertexType.ListedVertex);
                heap.Add(nextVertex);
            }
            else if (currentVertex.cost + currentEdge.weight < nextVertex.cost)
            {
                nextVertex.cost = currentVertex.cost + currentEdge.weight;
                heap.Update(nextVertex);
            }
            else
            {
                
                // We just ignore this edge.
            }
            if (ReferenceEquals(nextVertex, TargetVertex))
            {
                nextVertex.SetType(VertexType.EndVertex);
                PathFound = true;
                return false;
            }
            return true;
        }
    }
}
