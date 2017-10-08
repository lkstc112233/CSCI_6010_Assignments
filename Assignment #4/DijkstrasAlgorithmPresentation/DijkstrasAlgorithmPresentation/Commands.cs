using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstrasAlgorithmPresentation
{
    public class Commands
    {
        public static readonly RoutedUICommand LoadFileCommand = new RoutedUICommand("Load graph from file", "LoadFile", typeof(Commands));
        public static readonly RoutedUICommand AddVertexCommand = new RoutedUICommand("Add a new Vertex", "AddVertex", typeof(Commands));
        public static readonly RoutedUICommand AddEdgeCommand = new RoutedUICommand("Add a new Edge", "AddEdge", typeof(Commands));
        public static readonly RoutedUICommand RemoveVertexCommand = new RoutedUICommand("Remove this Vertex", "RemoveVertex", typeof(Commands));
        public static readonly RoutedUICommand RemoveEdgeCommand = new RoutedUICommand("Remove this Edge", "RemoveEdge", typeof(Commands));
        public static readonly RoutedUICommand ResetGraphCommand = new RoutedUICommand("Reset graph. Removing all Vertexes and Edges in it", "ResetGraph", typeof(Commands));
        public static readonly RoutedUICommand ConvertToDirectedCommand = new RoutedUICommand("Convert the graph to a directed graph", "ConvertToDirected", typeof(Commands));
        public static readonly RoutedUICommand ConvertToUndirectedCommand = new RoutedUICommand("Convert the graph to an undirected graph", "ConvertToUndirected", typeof(Commands));
        public static readonly RoutedUICommand SelectStartingPointCommand = new RoutedUICommand("Select starting Vertex for the algorithm", "SelectStartingPoint", typeof(Commands));
        public static readonly RoutedUICommand SelectEndingPointCommand = new RoutedUICommand("Select target Vertex for the algorithm", "SelectEndingPoint", typeof(Commands));

    }
}
