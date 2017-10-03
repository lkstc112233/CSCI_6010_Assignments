using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithmPresentation
{
    public class Edge
    {
        public Vertex start { get; set; }
        public Vertex end { get; set; }
        public double weight { get; set; }
    }
}
