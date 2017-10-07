using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DijkstrasAlgorithmPresentation
{
    enum SelectStatus
    {
        SelectAnElement, 
        SelectTargetVertex,
    }
    
    class ViewModelVertexEdge : INotifyPropertyChanged
    {
        public ViewModelVertexEdge()
        {
            m_graph = new Graph();
            m_graphModel = new GraphViewModelClass(m_graph);
        }

        private Vertex m_vertexSelected = null;
        public Vertex CurrentVertexSelected
        {
            get
            {
                return m_vertexSelected;
            }
            set
            {
                m_vertexSelected = value;
                onPropertyChanged("CurrentVertexSelected");
            }
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

        private Edge m_edgeSelected = null;
        public Edge CurrentEdgeSelected
        {
            get
            {
                return m_edgeSelected;
            }
            set
            {
                m_edgeSelected = value;
                onPropertyChanged("CurrentEdgeSelected");
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
    }

    public class EdgeViewModelClass : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Binding getOnewayBinding(object source, DependencyProperty property, IValueConverter converter)
        {
            if (source is ContentPresenter)
                if ((source as ContentPresenter).Content is Vertex)
                    return getBinding(source, new PropertyPath(property), converter, BindingMode.OneWay, (source as ContentPresenter).Content);
            return getBinding(source, new PropertyPath(property), converter, BindingMode.OneWay);
        }

        private Binding getBinding(object source, PropertyPath path, IValueConverter converter, BindingMode mode, object parameter = null)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = path;
            binding.Converter = converter;
            binding.ConverterParameter = parameter;
            binding.Mode = mode;
            return binding;
        }

        public EdgeViewModelClass(Edge e)
        {
            contentEdge = e;
            IValueConverter converter = new EclipseConverter();
            BindingOperations.SetBinding(this, X1Property, getOnewayBinding(startPresentser, Canvas.LeftProperty, converter));
            BindingOperations.SetBinding(this, X2Property, getOnewayBinding(endPresentser, Canvas.LeftProperty, converter));
            BindingOperations.SetBinding(this, Y1Property, getOnewayBinding(startPresentser, Canvas.TopProperty, converter));
            BindingOperations.SetBinding(this, Y2Property, getOnewayBinding(endPresentser, Canvas.TopProperty, converter));

            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(getOnewayBinding(startPresentser, Canvas.LeftProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(endPresentser, Canvas.LeftProperty, converter));
            multiBinding.Converter = new MinimalConverter();
            BindingOperations.SetBinding(this, LeftEdgeProperty, multiBinding);
            multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(getOnewayBinding(startPresentser, Canvas.TopProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(endPresentser, Canvas.TopProperty, converter));
            multiBinding.Converter = new MinimalConverter();
            BindingOperations.SetBinding(this, TopEdgeProperty, multiBinding);
        }

        private Edge contentEdge;
        public Edge edge => contentEdge;
        public Vertex start => edge.start;
        public Vertex end => edge.end;
        public double X1 { get { return (double)GetValue(X1Property); } set { SetValue(X1Property, value); } }
        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(EdgeViewModelClass));
        public double Y1 { get { return (double)GetValue(Y1Property); } set { SetValue(Y1Property, value); } }
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(EdgeViewModelClass));
        public double X2 { get { return (double)GetValue(X2Property); } set { SetValue(X2Property, value); } }
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(EdgeViewModelClass));
        public double Y2 { get { return (double)GetValue(Y2Property); } set { SetValue(Y2Property, value); } }
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(EdgeViewModelClass));
        public double LeftEdgeHere { get { return (double)GetValue(LeftEdgeProperty); } set { SetValue(LeftEdgeProperty, value); } }
        public static readonly DependencyProperty LeftEdgeProperty = DependencyProperty.Register("LeftEdgeHere", typeof(double), typeof(EdgeViewModelClass));
        public double TopEdgeHere { get { return (double)GetValue(TopEdgeProperty); } set { SetValue(TopEdgeProperty, value); } }
        public static readonly DependencyProperty TopEdgeProperty = DependencyProperty.Register("TopEdgeHere", typeof(double), typeof(EdgeViewModelClass));

        public ContentPresenter startPresentser => ViewModelVertexEdge.findVertexPresenter(start);
        public ContentPresenter endPresentser => ViewModelVertexEdge.findVertexPresenter(end);
    }

    public class GraphViewModelClass : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<EdgeViewModelClass> m_edgeModels = new ObservableCollection<EdgeViewModelClass>();
        public ObservableCollection<EdgeViewModelClass> edgeModels => m_edgeModels;
        private Dictionary<Edge, EdgeViewModelClass> modelPair = new Dictionary<Edge, EdgeViewModelClass>();

        public GraphViewModelClass(Graph g)
        {
            contentGraph = g;

            contentGraph.edges.CollectionChanged += Edges_CollectionChanged;
        }

        private void Edges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (contentGraph.edges != sender)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Edge edg in e.NewItems)
                        if (edg != null)
                        {
                            var ViewModel = new EdgeViewModelClass(edg);
                            modelPair.Add(edg, ViewModel);
                            edgeModels.Add(ViewModel);
                        }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Edge edg in e.OldItems)
                        if (edg != null)
                        {
                            edgeModels.Remove(modelPair[edg]);
                            modelPair.Remove(edg);
                        }
                    break;
            }
        }

        private Graph contentGraph;
        public Graph graph => contentGraph;

    }
}