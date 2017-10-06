using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmPresentation
{
    public class Graph
    {
        public List<Vertex> vertexes = new List<Vertex>();
        public List<Edge> edges = new List<Edge>();

        public Vertex createVertex()
        {
            Vertex LatestVertex = new Vertex();
            vertexes.Add(LatestVertex);
            LatestVertex.id = getNextAvailableVertexId();
            
            return LatestVertex;
        }
        public Edge AddEdge(Vertex vstart, Vertex vend)
        {
            Edge edg = new Edge();
            edg.start = vstart;
            edg.end = vend;
            edges.Add(edg);
            return edg;
        }

        internal int getNextAvailableVertexId()
        {
            return vertexes.Count();
        }
    }
}
