using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FinancialTsunamiPresentation
{
    public class Financial_Tsunami_data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Heap<Vertex> heap = new Heap<Vertex>((Vertex a, Vertex b) => { return a.cost < b.cost; });
        private Edge[] answerEdge;
        private Vertex[] answerVertex;
        private Graph graph;

        public delegate void AfterDelegate();
        public AfterDelegate OnPathFound;

        public Financial_Tsunami_data(Graph g)
        {
            graph = g;
            PathFound = false;
            answerEdge = new Edge[graph.EdgeTable.GetLength(0)];
            answerVertex = new Vertex[graph.EdgeTable.GetLength(0)];
        }
        public void setStartPoint(Vertex v)
        {
            DepartureVertex = v;
            v.cost = 0;
            heap.Add(v);
        } 
        public void setEndPoint(Vertex v)
        {
            TargetVertex = v;
        }

        Vertex DepartureVertex = null;
        Vertex TargetVertex = null;

        Vertex currentVertex = null;
        Edge currentEdge = null;
        Stack<Edge> appendingEdges = new Stack<Edge>();
        private bool m_pathFound = false;
        public bool PathFound
        {
            get { return m_pathFound; }
            set
            {
                m_pathFound = value;
                onPropertyChanged("PathFound");
                if (m_pathFound)
                    OnPathFound();
            }
        }

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
            {
                currentVertex.SetType(VertexType.ScannedVertex);
                ShowAnswers();
                PathFound = true;
                return null;
            }
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
            if (currentEdge == null)
            {
                if (TargetVertex != null)
                    ShowAnswers();
                return false;
            }
            currentEdge.SetType(EdgeType.ScanningEdge);
            Vertex nextVertex;
            
                nextVertex = currentEdge.end;
            
            if (nextVertex.cost < 0)
            {
                answerEdge[nextVertex.id] = currentEdge;
                answerVertex[nextVertex.id] = currentVertex;
                nextVertex.cost = currentVertex.cost + currentEdge.weight;
                nextVertex.SetType(VertexType.ListedVertex);
                heap.Add(nextVertex);
            }
            else if (currentVertex.cost + currentEdge.weight < nextVertex.cost)
            {
                answerEdge[nextVertex.id] = currentEdge;
                answerVertex[nextVertex.id] = currentVertex;
                nextVertex.cost = currentVertex.cost + currentEdge.weight;
                heap.Update(nextVertex);
            }
            return true;
        }

        private void ShowAnswers()
        {
            foreach (Vertex vertex in graph.vertexes)
                vertex.SetType(VertexType.NotPartOfAnswerVertex);
            foreach (Edge edge in graph.edges)
                edge.SetType(EdgeType.NotPartOfAnswerEdge);
            Vertex v;
            v = TargetVertex;
            if (v == null)
            {
                foreach (Edge e in answerEdge)
                {
                    if (e != null)
                    {
                        e.SetType(EdgeType.PartOfAnswerEdge);
                        e.start.SetType(VertexType.PartOfAnswerVertex);
                        e.end.SetType(VertexType.PartOfAnswerVertex);
                    }
                }
            }
            else
            {
                while (answerEdge[v.id] != null)
                {
                    answerEdge[v.id].SetType(EdgeType.PartOfAnswerEdge);
                    v = answerVertex[v.id];
                    v.SetType(VertexType.PartOfAnswerVertex);
                }
                TargetVertex.SetType(VertexType.EndVertex);
            }
            DepartureVertex.SetType(VertexType.StartingVertex);
        }
    }
}
