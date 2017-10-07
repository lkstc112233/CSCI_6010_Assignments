using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmPresentation
{
    public class Graph
    {
        private ObservableCollection<Vertex> m_vertexes = new ObservableCollection<Vertex>();
        private ObservableCollection<Edge> m_edges = new ObservableCollection<Edge>();

        private int vertexesIdIncreasmenter = 0;
        private int edgesIdIncreasmenter = 0;

        public ObservableCollection<Vertex> vertexes { get => m_vertexes; }
        public ObservableCollection<Edge> edges { get => m_edges; }

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
            edges.Add(edg);
            return edg;
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
