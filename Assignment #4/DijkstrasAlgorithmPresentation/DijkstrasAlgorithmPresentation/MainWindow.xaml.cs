using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DijkstrasAlgorithmPresentation
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModelVertexEdge viewModel;

        List<Ellipse> circles;
        List<Vertex> vertexes;

        Nullable<Point> dragStart = null;

        
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModelVertexEdge();
            DataContext = viewModel;

            moveStart = (varMoved, args) =>
            {
                var element = varMoved as UIElement;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            moveEnd = (varMoved, args) =>
            {
                var element = varMoved as UIElement;
                dragStart = null;
                element.ReleaseMouseCapture();
            };
            moving = (varMoved, args) =>
            {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = (UIElement)varMoved;
                    var p2 = args.GetPosition(monitor);
                    if (p2.X - dragStart.Value.X > 0)
                        Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                    else
                        Canvas.SetLeft(element, 0);
                    if (p2.Y - dragStart.Value.Y > 0)
                        Canvas.SetTop(element, p2.Y - dragStart.Value.Y);
                    else
                        Canvas.SetTop(element, 0);
                }
            };

            selectPresenter = (varMoved, args) =>
            {
                var presenter = varMoved as ContentPresenter;
                if (presenter.Content is Vertex)
                {
                    CancelSelection();
                    viewModel.CurrentVertexSelected = presenter.Content as Vertex;
                }
                if (presenter.Content is EdgeViewModelClass)
                {
                    CancelSelection();
                    viewModel.CurrentEdgeSelected = (presenter.Content as EdgeViewModelClass).edge;
                }
                args.Handled = true;
            };

        }

        private void Add_Circle_Button_Click(object sender, RoutedEventArgs e)
        {
            if (circles == null)
                circles = new List<Ellipse>();
            Ellipse newOne = new Ellipse() { Fill = Brushes.Red, Width = 20, Height = 20 };

            // Enable drag.
            newOne.MouseDown += moveStart;
            newOne.MouseUp += moveEnd;
            newOne.MouseMove += moving;
            

            monitor.Children.Add(newOne);
            Canvas.SetLeft(newOne, 10);
            Canvas.SetTop(newOne, 10);
            Canvas.SetZIndex(newOne, 10);

            if (circles.Count == 0)
            {
                circles.Add(newOne);
                return;
            }

            Ellipse last = circles.Last();
            circles.Add(newOne);
            Line connection = new Line() { Stroke = Brushes.Cyan, StrokeThickness = 2 };
            Binding binding = new Binding();
            binding.Source = last;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.X1Property, binding);
            binding = new Binding();
            binding.Source = newOne;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.X2Property, binding);
            binding = new Binding();
            binding.Source = last;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.Y1Property, binding);
            binding = new Binding();
            binding.Source = newOne;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.Y2Property, binding);

            monitor.Children.Add(connection);
        }

        private void Set_Vertex_try(object sender, RoutedEventArgs e)
        {
            viewModel.CurrentVertexSelected = new Vertex();
            viewModel.CurrentVertexSelected.color = Colors.Green;
            viewModel.CurrentVertexSelected = viewModel.CurrentVertexSelected;
        }

        private void Make_Vertex_Null(object sender, RoutedEventArgs e)
        {
            viewModel.CurrentVertexSelected = null;
        }

        MouseButtonEventHandler moveStart;
        MouseButtonEventHandler moveEnd;
        MouseEventHandler moving;
        
        MouseButtonEventHandler selectPresenter;
        MouseButtonEventHandler vertexMouseUp;
        MouseEventHandler vertexMouseMoving;

        private void Add_Vertex(object sender, RoutedEventArgs e)
        {
            var rd = new ResourceDictionary();
            rd.Source = new Uri("ControlPanelDisplayDictionary.xaml", UriKind.RelativeOrAbsolute);
            if (vertexes == null)
                vertexes = new List<Vertex>();
            Vertex last = null;
            if (vertexes.Count > 0)
                last = vertexes.Last();
            vertexes.Add(new Vertex());
            vertexes.Last().id = vertexes.Count;
            viewModel.CurrentVertexSelected = vertexes.Last();
            var cont = new ContentPresenter();
            cont.ContentTemplate = (DataTemplate)rd["VertexNode"];
            cont.Content = vertexes.Last();

            cont.MouseDown += moveStart;
            cont.MouseDown += selectPresenter;
            cont.MouseUp += moveEnd;
            cont.MouseMove += moving;

            ViewModelVertexEdge.vertexPresenterDictionary.Add(vertexes.Last(), cont);
            Canvas.SetLeft(ViewModelVertexEdge.vertexPresenterDictionary[vertexes.Last()], 10);
            Canvas.SetTop(ViewModelVertexEdge.vertexPresenterDictionary[vertexes.Last()], 10);

            monitor.Children.Add(cont);
            if (last != null)
            {
                Edge edg = new Edge();
                edg.start = last;
                edg.end = vertexes.Last();
                cont = new ContentPresenter();
                cont.ContentTemplate = (DataTemplate)rd["EdgePresent"];
                cont.Content = new EdgeViewModelClass(edg);
                cont.MouseDown += selectPresenter;
                monitor.Children.Add(cont);
            }
        }

        private void CancelSelection(object sender, MouseButtonEventArgs e)
        {
            CancelSelection();
        }

        private void CancelSelection()
        {
            viewModel.CurrentVertexSelected = null;
            viewModel.CurrentEdgeSelected = null;
        }
    }


}
