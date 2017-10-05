using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        public EdgeViewModelClass(Edge e)
        {
            contentEdge = e;
            Binding binding = new Binding();
            binding.Source = startPresentser;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this, X1Property, binding);
            binding = new Binding();
            binding.Source = endPresentser;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this, X2Property, binding);
            binding = new Binding();
            binding.Source = startPresentser;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this, Y1Property, binding);
            binding = new Binding();
            binding.Source = endPresentser;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this, Y2Property, binding);
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

        public ContentPresenter startPresentser => ViewModelVertexEdge.vertexPresenterDictionary[start];
        public ContentPresenter endPresentser => ViewModelVertexEdge.vertexPresenterDictionary[end];
    }
}