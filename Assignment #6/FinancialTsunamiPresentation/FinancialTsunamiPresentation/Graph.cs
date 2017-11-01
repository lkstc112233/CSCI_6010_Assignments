using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTsunamiPresentation
{
    public class Graph : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private ObservableCollection<Vertex> m_vertexes = new ObservableCollection<Vertex>();
        private ObservableCollection<Edge> m_edges = new ObservableCollection<Edge>();

        private int vertexesIdIncreasmenter = 0;
        private int edgesIdIncreasmenter = 0;

        private int currentMaxiumCapacity = 0;

        public ObservableCollection<Vertex> vertexes { get => m_vertexes; }
        public ObservableCollection<Edge> edges { get => m_edges; }

        public Edge[,] EdgeTable;

        private bool m_CanToDirect = false;
        public bool CanToDirect { get { return m_CanToDirect; } }
        private bool m_CanToUndirect = true;
        public bool CanToUndirect { get { return m_CanToUndirect; } }

        public Graph()
        {
            currentMaxiumCapacity = 10;
            EdgeTable = new Edge[currentMaxiumCapacity, currentMaxiumCapacity];
        }

        public void ClearGraph()
        {
            for (int i = 0; i < currentMaxiumCapacity; ++i)
                for (int j = 0; j < currentMaxiumCapacity; ++j)
                    EdgeTable[i, j] = null;
            edges.Clear();
            vertexes.Clear();
            vertexesIdIncreasmenter = 0;
            edgesIdIncreasmenter = 0;
        }
        
        public Vertex createVertex()
        {
            Vertex LatestVertex = new Vertex();
            LatestVertex.id = getNextAvailableVertexId();
            vertexes.Add(LatestVertex);
            
            return LatestVertex;
        }

        public void RemoveVertex(Vertex v)
        {
            List<Edge> toRemove = new List<Edge>();
            foreach (Edge e in edges)
                if (e.start == v || e.end == v)
                    toRemove.Add(e);
            foreach (Edge e in toRemove)
                RemoveEdge(e);
            vertexes.Remove(v);
        }

        public Edge AddEdge(Vertex vstart, Vertex vend)
        {
            if (EdgeTable[vstart.id, vend.id] != null)
                return null;
            Edge edg = new Edge();
            edg.start = vstart;
            edg.end = vend;
            edg.id = getNextAvailableEdgeId();
            AddEdge(edg);
            return edg;
        }

        private void AddEdge(Edge e)
        {
            edges.Add(e);
            EdgeTable[e.start.id, e.end.id] = e;
        }

        public void RemoveEdge(Edge e)
        {
            EdgeTable[e.start.id, e.end.id] = null;
            e.start = null; // Deregister listener.
            edges.Remove(e);
        }

        private void ExpandCapacity()
        {
            int temp = currentMaxiumCapacity * 2;
            Edge[,] tempEdges = new Edge[temp, temp];
            for (int i = 0; i < currentMaxiumCapacity; ++i)
                for (int j = 0; j < currentMaxiumCapacity; ++j)
                    tempEdges[i, j] = EdgeTable[i, j];

            EdgeTable = tempEdges;
            currentMaxiumCapacity = temp;
        }

        private int getNextAvailableVertexId()
        {
            if (++vertexesIdIncreasmenter >= currentMaxiumCapacity)
                ExpandCapacity();
            return vertexesIdIncreasmenter;
        }

        private int getNextAvailableEdgeId()
        {
            return ++edgesIdIncreasmenter;
        }

        internal void ResetCosts()
        {
            foreach (Vertex v in vertexes)
            {
                v.safe = true;
            }
        }
    }
}
