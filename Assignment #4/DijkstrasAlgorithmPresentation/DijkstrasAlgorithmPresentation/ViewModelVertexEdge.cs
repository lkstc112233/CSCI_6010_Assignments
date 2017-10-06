using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DijkstrasAlgorithmPresentation
{
    class ViewModelVertexEdge : INotifyPropertyChanged
    {
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

        public static Dictionary<Vertex, ContentPresenter> vertexPresenterDictionary = new Dictionary<Vertex, ContentPresenter>();


        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    class EdgeViewModelClass : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Binding getOnewayBinding(object source, DependencyProperty property, IValueConverter converter)
        {
            return getBinding(source, new PropertyPath(property), converter, BindingMode.OneWay);
        }

        private Binding getBinding(object source, PropertyPath path, IValueConverter converter, BindingMode mode)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = path;
            binding.Converter = converter;
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

        public ContentPresenter startPresentser => ViewModelVertexEdge.vertexPresenterDictionary[start];
        public ContentPresenter endPresentser => ViewModelVertexEdge.vertexPresenterDictionary[end];
    }
}