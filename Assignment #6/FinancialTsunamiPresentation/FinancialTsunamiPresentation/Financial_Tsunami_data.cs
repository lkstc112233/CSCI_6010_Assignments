using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FinancialTsunamiPresentation
{
    public class Financial_Tsunami_data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        private Edge[] answerEdge;
        private Vertex[] answerVertex;
        private Graph graph;

        public delegate void AfterDelegate();
        public AfterDelegate OnPathFound;

        public Financial_Tsunami_data(Graph g)
        {
            graph = g;
            PathFound = false;
            answerEdge = new Edge[graph.EdgeTable.GetLength(0)];
            answerVertex = new Vertex[graph.EdgeTable.GetLength(0)];
        }

        Vertex currentVertex = null;
        Edge currentEdge = null;
        Stack<Edge> appendingEdges = new Stack<Edge>();
        private bool m_pathFound = false;
        public bool PathFound
        {
            get { return m_pathFound; }
            set
            {
                m_pathFound = value;
                onPropertyChanged("PathFound");
                if (m_pathFound)
                    OnPathFound();
            }
        }

        public void ResetStatus()
        {
            currentEdge = null;
            currentEdge = null;
            appendingEdges.Clear();
        }

        public bool OneStep()
        {
            if (PathFound)
                return false;
            return true;
        }

        private void ShowAnswers()
        {
        }
    }
}
