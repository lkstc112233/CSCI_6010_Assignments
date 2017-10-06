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

        Graph graph = new Graph();

        Nullable<Point> dragStart = null;

        ResourceDictionary ControlPanelDisplayDictionary = new ResourceDictionary();

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModelVertexEdge();
            DataContext = viewModel;

            ControlPanelDisplayDictionary.Source = new Uri("ControlPanelDisplayDictionary.xaml", UriKind.RelativeOrAbsolute);


            // These lambda functions are for draging Vertex nodes around.
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

            // This function is for selecting elements on the screen.
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

        MouseButtonEventHandler moveStart;
        MouseButtonEventHandler moveEnd;
        MouseEventHandler moving;
        
        MouseButtonEventHandler selectPresenter;

        private void Add_Vertex(object sender, RoutedEventArgs e)
        {
            Vertex last = null;
            if (graph.vertexes.Count > 0)
                last = graph.vertexes.Last();

            Vertex LatestVertex = CreateVertex();

            if (last != null)
                AddEdge(last, LatestVertex);
        }

        public Vertex CreateVertex()
        {
            Vertex LatestVertex = graph.createVertex();
            viewModel.CurrentVertexSelected = LatestVertex;
            var cont = new ContentPresenter();
            cont.ContentTemplate = (DataTemplate)ControlPanelDisplayDictionary["VertexNode"];
            cont.Content = LatestVertex;

            cont.MouseDown += moveStart;
            cont.MouseDown += selectPresenter;
            cont.MouseUp += moveEnd;
            cont.MouseMove += moving;
            Canvas.SetZIndex(cont, 10);

            ViewModelVertexEdge.vertexPresenterDictionary.Add(LatestVertex, cont);
            Canvas.SetLeft(cont, 10);
            Canvas.SetTop(cont, 10);

            monitor.Children.Add(cont);
            return LatestVertex;
        }


        public Edge AddEdge(Vertex vstart, Vertex vend)
        {
            Edge edg = graph.AddEdge(vstart, vend);
            ContentPresenter cont = new ContentPresenter();
            cont.ContentTemplate = (DataTemplate)ControlPanelDisplayDictionary["EdgePresent"];
            cont.Content = new EdgeViewModelClass(edg);
            cont.MouseDown += selectPresenter;
            Canvas.SetZIndex(cont, 5);
            monitor.Children.Add(cont);
            return edg;
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
