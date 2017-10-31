using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FinancialTsunamiPresentation
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
        public Vertex CurrentVertexSelected
        {
            get => m_vertexSelected;
            private set
            {
                m_vertexSelected = value;
                onPropertyChanged("CurrentVertexSelected");
            }
        }

        public Vertex VertexStarting
        {
            get => m_vertexStarting;
            private set
            {
                m_vertexStarting = value;
                onPropertyChanged("VertexStarting");
            }
        }

        public Vertex VertexEnd
        {
            get => m_vertexEnd;
            private set
            {
                m_vertexEnd = value;
                onPropertyChanged("VertexEnd");
            }
        }
        public void SelectVertex(Vertex v)
        {
            if (CurrentVertexSelected != null)
                CancelVertexSelection();
            CurrentVertexSelected = v;
            CurrentVertexSelected.Select();
        }
        internal void SelectStartVertex(Vertex vertex)
        {
            if (VertexStarting != null && VertexStarting != VertexEnd)
                VertexStarting.SetType(VertexType.Unselected);
            VertexStarting = vertex;
            VertexStarting.SetType(VertexType.StartingVertex);
        }
        internal void SelectEndVertex(Vertex vertex)
        {
            if (VertexEnd != null && VertexStarting != VertexEnd)
                VertexEnd.SetType(VertexType.Unselected);
            VertexEnd = vertex;
            VertexEnd.SetType(VertexType.EndVertex);
        }
        internal void CancelSelectEndVertex()
        {
            if (VertexEnd == null)
                return;
            if (VertexStarting != VertexEnd)
                VertexEnd.SetType(VertexType.Unselected);
            VertexEnd = null;
        }
        public void CancelVertexSelection()
        {
            if (CurrentVertexSelected != null)
                CurrentVertexSelected.NotSelect();
            CurrentVertexSelected = null;
        }
        public void BeginEdgeBuilding(Vertex v)
        {
            CurrentVertexSelected = v;
            CurrentVertexSelected.BuildEdge();
            CurrentStatus = SelectStatus.EdgeBuilding;
        }

        private Edge m_edgeSelected = null;
        public Edge CurrentEdgeSelected
        {
            get => m_edgeSelected;
            private set
            {
                m_edgeSelected = value;
                onPropertyChanged("CurrentEdgeSelected");
            }
        }
        public void SelectEdge(Edge e)
        {
            if (CurrentEdgeSelected != null)
                CancelEdgeSelection();
            CurrentEdgeSelected = e;
            CurrentEdgeSelected.SelectEdge();
        }
        public void CancelEdgeSelection()
        {
            if (CurrentEdgeSelected != null)
                CurrentEdgeSelected.NotSelectEdge();
            CurrentEdgeSelected = null;
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

        private SelectStatus m_CurrentStatus = SelectStatus.SelectAnElement;
        public SelectStatus CurrentStatus
        {
            get
            {
                return m_CurrentStatus;
            }
            set
            {
                m_CurrentStatus = value;
                onPropertyChanged("CurrentStatus");
            }
        }

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

        private Financial_Tsunami_data m_AlgorithmData;
        public Financial_Tsunami_data AlgorithmData
        {
            get
            {
                return m_AlgorithmData;
            }
            set
            {
                m_AlgorithmData = value;
                onPropertyChanged("AlgorithmData");
            }
        }

        private bool m_PathFound;
        public bool PathFound
        {
            get
            {
                return m_PathFound;
            }
            set
            {
                m_PathFound = value;
                onPropertyChanged("PathFound");
            }
        }

        internal void BeginPresentation()
        {
            if (VertexStarting == null)
                return;
            CurrentProgramStatus = ProgramStatus.Presenting;

            PathFound = false;
            AlgorithmData = new Financial_Tsunami_data(graphModel.graph);
            AlgorithmData.OnPathFound = () => PathFound = true;
            AlgorithmData.setStartPoint(VertexStarting);
            AlgorithmData.setEndPoint(VertexEnd);
        }

        internal void EndPresentation()
        {
            if (CurrentProgramStatus == ProgramStatus.BuildingGraph)
                return;
            CurrentProgramStatus = ProgramStatus.BuildingGraph;

            AlgorithmData.ResetStatus();
            graphModel.graph.ResetCosts();
            if (VertexStarting != null)
                VertexStarting.SetType(VertexType.StartingVertex);
            if (VertexEnd != null)
                VertexEnd.SetType(VertexType.EndVertex);
        }

        internal void ClearGraph()
        {
            graphModel.graph.ClearGraph();
            CurrentEdgeSelected = null;
            CurrentVertexSelected = null;
            VertexStarting = null;
            VertexEnd = null;
        }
    }
}