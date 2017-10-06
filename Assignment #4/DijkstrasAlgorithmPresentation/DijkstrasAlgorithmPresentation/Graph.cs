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

        internal int getNextAvailableVertexId()
        {
            return vertexes.Count();
        }
    }
}
