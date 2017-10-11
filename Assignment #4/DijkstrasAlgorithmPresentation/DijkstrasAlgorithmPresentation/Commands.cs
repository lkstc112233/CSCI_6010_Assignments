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
        
        public static readonly RoutedUICommand BeginPresentationCommand = new RoutedUICommand("Begin the presentation", "BeginPresentation", typeof(Commands));
        public static readonly RoutedUICommand EndPresentationCommand = new RoutedUICommand("Terminate the presentation", "EndPresentation", typeof(Commands));
        public static readonly RoutedUICommand SolvePresentationCommand = new RoutedUICommand("Start solving the problem step by step", "SolvePresentation", typeof(Commands));
        public static readonly RoutedUICommand StopSolvePresentationCommand = new RoutedUICommand("Start solving the problem step by step", "SolvePresentation", typeof(Commands));

    }
}
