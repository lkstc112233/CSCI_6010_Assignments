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
        public static readonly RoutedUICommand AddEdgeCommand = new RoutedUICommand("Add a new Edge", "AddEdge", typeof(Commands));
        public static readonly RoutedUICommand RemoveVertexCommand = new RoutedUICommand("Remove this Vertex", "RemoveVertex", typeof(Commands));
        public static readonly RoutedUICommand RemoveEdgeCommand = new RoutedUICommand("Remove this Edge", "RemoveEdge", typeof(Commands));

    }
}
