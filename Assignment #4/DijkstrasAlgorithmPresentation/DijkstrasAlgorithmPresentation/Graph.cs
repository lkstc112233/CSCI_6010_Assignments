using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmPresentation
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

        private bool DefaultEdgeTypeOneway = true;

        private bool m_CanToDirect = false;
        public bool CanToDirect { get { return m_CanToDirect; } }
        private bool m_CanToUndirect = true;
        public bool CanToUndirect { get { return m_CanToUndirect; } }

        public Graph()
        {
            currentMaxiumCapacity = 10;
            EdgeTable = new Edge[currentMaxiumCapacity, currentMaxiumCapacity];
        }

        public void NowCanToDirect()
        {
            m_CanToDirect = true;
            NotifyPropertyChanged("CanToDirect");
        }

        public void NowCanToUndirect()
        {
            m_CanToUndirect = true;
            NotifyPropertyChanged("CanToUndirect");
        }
        public void NowCannotToDirect()
        {
            m_CanToDirect = false;
            NotifyPropertyChanged("CanToDirect");
        }

        public void NowCannotToUndirect()
        {
            m_CanToUndirect = false;
            NotifyPropertyChanged("CanToUndirect");
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
            edg.oneway = DefaultEdgeTypeOneway;
            AddEdge(edg);
            return edg;
        }

        private void AddEdge(Edge e)
        {
            edges.Add(e);
            if (!e.oneway)
                EdgeTable[e.end.id, e.start.id] = e;
            EdgeTable[e.start.id, e.end.id] = e;
        }

        public void RemoveEdge(Edge e)
        {
            if (!e.oneway)
                EdgeTable[e.end.id, e.start.id] = null;
            EdgeTable[e.start.id, e.end.id] = null;
            edges.Remove(e);
        }

        public void ToUndirectedGraph()
        {
            NowCannotToUndirect();
            NowCanToDirect();
            DefaultEdgeTypeOneway = false;
            List<Edge> duplicates = new List<Edge>();
            foreach (Edge e in edges)
            {
                if (e.oneway)
                    if (EdgeTable[e.end.id, e.start.id] == null)
                        EdgeTable[e.end.id, e.start.id] = e;
                    else if (e.start.id > e.end.id)
                    {
                        duplicates.Add(e);
                        continue;
                    }
                    else
                        EdgeTable[e.end.id, e.start.id] = e;
                e.oneway = false;
            }
            foreach (Edge e in duplicates)
                edges.Remove(e);
        }

        public void ToDirectedGraph()
        {
            NowCanToUndirect();
            NowCannotToDirect();
            DefaultEdgeTypeOneway = true;
            foreach (Edge e in edges)
            {
                if (!e.oneway)
                    EdgeTable[e.end.id, e.start.id] = null;
                e.oneway = true;
            }
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
    }
}
