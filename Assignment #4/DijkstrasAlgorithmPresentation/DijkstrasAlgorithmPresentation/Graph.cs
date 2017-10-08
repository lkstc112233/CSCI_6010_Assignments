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

        public ObservableCollection<Vertex> vertexes { get => m_vertexes; }
        public ObservableCollection<Edge> edges { get => m_edges; }

        private bool DefaultEdgeTypeOneway = true;

        private bool m_CanToDirect = false;
        public bool CanToDirect { get { return m_CanToDirect; } }
        private bool m_CanToUndirect = true;
        public bool CanToUndirect { get { return m_CanToUndirect; } }

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
        public Edge AddEdge(Vertex vstart, Vertex vend)
        {
            Edge edg = new Edge();
            edg.start = vstart;
            edg.end = vend;
            edg.id = getNextAvailableEdgeId();
            edg.oneway = DefaultEdgeTypeOneway;
            edges.Add(edg);
            return edg;
        }

        public void ToUndirectedGraph()
        {
            NowCannotToUndirect();
            NowCanToDirect();
            DefaultEdgeTypeOneway = false;
            foreach (Edge e in edges)
            {
                e.oneway = false;
            }
        }

        public void ToDirectedGraph()
        {
            NowCanToUndirect();
            NowCannotToDirect();
            DefaultEdgeTypeOneway = true;
            foreach (Edge e in edges)
            {
                e.oneway = true;
            }
        }

        private int getNextAvailableVertexId()
        {
            return ++vertexesIdIncreasmenter;
        }

        private int getNextAvailableEdgeId()
        {
            return ++edgesIdIncreasmenter;
        }
    }
}
