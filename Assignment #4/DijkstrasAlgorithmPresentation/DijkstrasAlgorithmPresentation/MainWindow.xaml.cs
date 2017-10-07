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

        Nullable<Point> dragStart = null;

        ResourceDictionary ControlPanelDisplayDictionary = new ResourceDictionary();

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModelVertexEdge();
            DataContext = viewModel;

            ViewModelVertexEdge.graphControl = monitor;

            ControlPanelDisplayDictionary.Source = new Uri("ControlPanelDisplayDictionary.xaml", UriKind.RelativeOrAbsolute);

            UIElement cvs = monitor as UIElement;

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
                    var p2 = args.GetPosition(cvs);
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
                if (viewModel.CurrentStatus == SelectStatus.SelectAnElement)
                {
                    if (presenter.Content is Vertex)
                    {
                        CancelSelectionAndResetStatus();
                        viewModel.CurrentVertexSelected = presenter.Content as Vertex;
                        args.Handled = true;
                    }
                    if (presenter.Content is EdgeViewModelClass)
                    {
                        CancelSelectionAndResetStatus();
                        viewModel.CurrentEdgeSelected = (presenter.Content as EdgeViewModelClass).edge;
                        args.Handled = true;
                    }
                }
                else if (viewModel.CurrentStatus == SelectStatus.SelectTargetVertex)
                {
                    if (presenter.Content is Vertex)
                    {
                        Edge e = AddEdge(viewModel.CurrentVertexSelected , presenter.Content as Vertex);
                        CancelSelectionAndResetStatus();
                        viewModel.CurrentEdgeSelected = e;
                        args.Handled = true;

                    }
                }
            };
            

        }

        MouseButtonEventHandler moveStart;
        MouseButtonEventHandler moveEnd;
        MouseEventHandler moving;
        
        MouseButtonEventHandler selectPresenter;

        private void Add_Vertex(object sender, RoutedEventArgs e)
        {
            Vertex LatestVertex = CreateVertex();
            viewModel.CurrentVertexSelected = LatestVertex;


        }

        public Vertex CreateVertex()
        {
            Vertex LatestVertex = viewModel.graphModel.graph.createVertex();

            return LatestVertex;
        }


        public Edge AddEdge(Vertex vstart, Vertex vend)
        {
            Edge edg = viewModel.graphModel.graph.AddEdge(vstart, vend);

            return edg;
        }

        private void CancelSelectionAndResetStatus(object sender, MouseButtonEventArgs e)
        {
            CancelSelectionAndResetStatus();
        }

        private void CancelSelectionAndResetStatus()
        {
            viewModel.CurrentVertexSelected = null;
            viewModel.CurrentEdgeSelected = null;
            viewModel.CurrentStatus = SelectStatus.SelectAnElement;
        }

        private void AddEdge(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is Button)
            {
                Button button = e.OriginalSource as Button;
                if (button.Tag is Vertex)
                {
                    Vertex last = button.Tag as Vertex;
                    viewModel.CurrentStatus = SelectStatus.SelectTargetVertex;
                }
            }
        }

        private void CanAddEdge(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (viewModel.CurrentStatus == SelectStatus.SelectAnElement);
        }

        private void RemoveEdge(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: remove the edge.
        }

        private void CanRemoveEdge(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }


}
