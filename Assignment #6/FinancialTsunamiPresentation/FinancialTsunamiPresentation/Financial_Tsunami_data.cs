using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FinancialTsunamiPresentation
{
    public class Financial_Tsunami_data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public double limit { get; set; } = 200;
        private Graph graph;

        public delegate void AfterDelegate();
        public AfterDelegate OnPathFound;

        public Financial_Tsunami_data(Graph g)
        {
            graph = g;
            PathFound = false;
        }

        Vertex currentVertex = null;
        List<Vertex> listToCheck = null;
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
            currentVertex = null;
            listToCheck = null;
        }

        private void reloadVertexes()
        {
            listToCheck = graph.vertexes.Where(vtx => vtx.safe).ToList();
        }

        private Vertex getNextVertex()
        {
            if (listToCheck == null)
                reloadVertexes();
            if (listToCheck.Count == 0)
                return null;
            Vertex result = listToCheck[listToCheck.Count - 1];
            listToCheck.RemoveAt(listToCheck.Count - 1);
            return result;
        }

        public bool OneStep()
        {
            if (PathFound)
                return false;
            currentVertex = getNextVertex();
            if (currentVertex == null)
            {
                PathFound = true;
                return false;
            }
            double balance = currentVertex.balance;
            for (int i = 0; i < graph.EdgeTable.GetLength(0); ++i)
                if (graph.EdgeTable[currentVertex.id, i] != null)
                    if (graph.EdgeTable[currentVertex.id, i].end.safe)
                        balance += graph.EdgeTable[currentVertex.id, i].weight;
            if (balance < limit)
            {
                currentVertex.safe = false;
                reloadVertexes();
            }
            return true;
        }

        private void ShowAnswers()
        {
        }
    }
}
