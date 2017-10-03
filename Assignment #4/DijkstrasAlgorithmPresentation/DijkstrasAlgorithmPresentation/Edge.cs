using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmPresentation
{
    class Edge
    {
        public Vertex start { get; }
        public Vertex end { get; }
        public double weight { get; set; }
    }
}
