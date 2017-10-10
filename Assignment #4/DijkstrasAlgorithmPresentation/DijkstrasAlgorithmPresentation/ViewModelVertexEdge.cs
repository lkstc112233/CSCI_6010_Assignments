using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DijkstrasAlgorithmPresentation
{
    enum SelectStatus
    {
        SelectAnElement,
        EdgeBuilding,
        SelectAStartingVertex,
        SelectAnEndVertex,
    }

    enum ProgramStatus
    {
        BuildingGraph,
        Presenting,
    }

    class ViewModelVertexEdge : INotifyPropertyChanged
    {
        public ViewModelVertexEdge()
        {
            m_graph = new Graph();
            m_graphModel = new GraphViewModelClass(m_graph);
        }

        private ProgramStatus m_ProgramStatus = ProgramStatus.BuildingGraph;
        public ProgramStatus CurrentProgramStatus
        {
            get
            {
                return m_ProgramStatus;
            }
            set
            {
                m_ProgramStatus = value;
                onPropertyChanged("CurrentProgramStatus");
            }
        }
        private Vertex m_vertexSelected = null;
        private Vertex m_vertexStarting = null;
        private Vertex m_vertexEnd = null;
        public Vertex CurrentVertexSelected => m_vertexSelected;
        public void SelectVertex(Vertex v)
        {
            if (m_vertexSelected != null)
                CancelVertexSelection();
            m_vertexSelected = v;
            m_vertexSelected.Select();
            onPropertyChanged("CurrentVertexSelected");
        }
        internal void SelectStartVertex(Vertex vertex)
        {
            if (m_vertexStarting != null && m_vertexStarting != m_vertexEnd)
                m_vertexStarting.SetType(VertexType.Unselected);
            m_vertexStarting = vertex;
            m_vertexStarting.SetType(VertexType.StartingVertex);
            onPropertyChanged("CurrentVertexSelected");
        }
        internal void SelectEndVertex(Vertex vertex)
        {
            if (m_vertexEnd != null && m_vertexStarting != m_vertexEnd)
                m_vertexEnd.SetType(VertexType.Unselected);
            m_vertexEnd = vertex;
            m_vertexEnd.SetType(VertexType.EndVertex);
            onPropertyChanged("CurrentVertexSelected");
        }
        public void CancelVertexSelection()
        {
            if (m_vertexSelected != null)
                m_vertexSelected.NotSelect();
            m_vertexSelected = null;
            onPropertyChanged("CurrentVertexSelected");
        }
        public void BeginEdgeBuilding(Vertex v)
        {
            m_vertexSelected = v;
            m_vertexSelected.BuildEdge();
            CurrentStatus = SelectStatus.EdgeBuilding;
            onPropertyChanged("CurrentVertexSelected");
        }

        private Edge m_edgeSelected = null;
        public Edge CurrentEdgeSelected => m_edgeSelected;
        public void SelectEdge(Edge e)
        {
            if (m_edgeSelected != null)
                CancelEdgeSelection();
            m_edgeSelected = e;
            m_edgeSelected.color = Colors.Aqua;
            onPropertyChanged("CurrentEdgeSelected");
        }
        public void CancelEdgeSelection()
        {
            if (m_edgeSelected != null)
                m_edgeSelected.color = Colors.Blue;
            m_edgeSelected = null;
            onPropertyChanged("CurrentEdgeSelected");
        }

        private Graph m_graph;
        private GraphViewModelClass m_graphModel;
        public GraphViewModelClass graphModel
        {
            get
            {
                return m_graphModel;
            }
            set
            {
                m_graphModel = value;
                onPropertyChanged("graphModel");
            }
        }


        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        return (T)child;

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        public static T FindVisualChild<T>(DependencyObject depObj, string name) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        if (child is Control)
                            if ((child as Control).Name.Equals(name))
                                return (T)child;

                    T childItem = FindVisualChild<T>(child, name);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        private static Dictionary<Vertex, ContentPresenter> vertexPresenterDictionary = new Dictionary<Vertex, ContentPresenter>();
        public static ContentPresenter findVertexPresenter(Vertex v)
        {
            if (!vertexPresenterDictionary.ContainsKey(v))
            {
                var temp = FindVisualChild<ItemsControl>(graphControl, "VertexesControl");
                var temp2 = temp.ItemContainerGenerator.ContainerFromItem(v) as ContentPresenter;
                if (temp2 == null) throw new InvalidOperationException();
                vertexPresenterDictionary[v] = temp2;
            }
            return vertexPresenterDictionary[v];
        }
        public SelectStatus CurrentStatus = SelectStatus.SelectAnElement;

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static ContentPresenter graphControl = null;

        public void RemoveCurrentSelectedVertex()
        {
            if (CurrentVertexSelected == null)
                return;
            graphModel.graph.RemoveVertex(CurrentVertexSelected);
        }

        public void RearrangeVertexes()
        {
            int count = graphModel.graph.vertexes.Count;
            if (count <= 1)
                return;
            double step = 2 * Math.PI / count;
            double x0 = 180;
            double y0 = 150;
            double r = 130;
            for (int i = 0; i < count; ++i)
            {
                var presenter = findVertexPresenter(graphModel.graph.vertexes[i]);
                Canvas.SetLeft(presenter, x0 + r * Math.Sin(step * i));
                Canvas.SetTop(presenter, y0 + r * Math.Cos(step * i));
            }
        }

        public Dijkstra_s_Algorithm_data AlgorithmData;

        internal void BeginPresentation()
        {
            if (!CanBeginPresentation())
                return;
            CurrentProgramStatus = ProgramStatus.Presenting;

            // TODO: preparation.

            AlgorithmData = new Dijkstra_s_Algorithm_data(graphModel.graph);
            AlgorithmData.setStartPoint(m_vertexStarting);
            AlgorithmData.setEndPoint(m_vertexEnd);
        }

        internal void EndPresentation()
        {
            if (CurrentProgramStatus == ProgramStatus.BuildingGraph)
                return;
            CurrentProgramStatus = ProgramStatus.BuildingGraph;
            // TODO.

            AlgorithmData.ResetStatus();
            graphModel.graph.ResetCosts();
        }

        internal void ClearGraph()
        {
            graphModel.graph.ClearGraph();
            m_edgeSelected = null;
            m_vertexSelected = null;
            m_vertexStarting = null;
            m_vertexEnd = null;
        }

        internal bool CanBeginPresentation()
        {
            return m_vertexStarting != null && m_vertexEnd != null;
        }
    }
}