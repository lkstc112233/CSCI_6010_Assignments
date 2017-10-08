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
                        viewModel.SelectVertex(presenter.Content as Vertex);
                        args.Handled = true;
                    }
                    if (presenter.Content is EdgeViewModelClass)
                    {
                        CancelSelectionAndResetStatus();
                        viewModel.SelectEdge((presenter.Content as EdgeViewModelClass).edge);
                        args.Handled = true;
                    }
                }
                else if (viewModel.CurrentStatus == SelectStatus.EdgeBuilding)
                {
                    if (presenter.Content is Vertex)
                    {
                        if (presenter.Content != viewModel.CurrentVertexSelected)
                        {
                            Edge e = AddEdge(viewModel.CurrentVertexSelected, presenter.Content as Vertex);
                            CancelSelectionAndResetStatus();
                            if (e != null)
                                viewModel.SelectEdge(e);
                            args.Handled = true;
                        }
                        else
                        {
                            CancelSelectionAndResetStatus();
                        }
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
            viewModel.SelectVertex(LatestVertex);
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
            viewModel.CancelVertexSelection();
            viewModel.CancelEdgeSelection();
            viewModel.CurrentStatus = SelectStatus.SelectAnElement;
        }

        private void AddEdge(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is Button)
            {
                Button button = e.OriginalSource as Button;
                if (button.Tag is Vertex)
                    viewModel.BeginEdgeBuilding(button.Tag as Vertex);
            }
        }

        private void CanAddEdge(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (viewModel.CurrentStatus == SelectStatus.SelectAnElement);
        }

        private void RemoveEdge(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is Button)
            {
                Button button = e.OriginalSource as Button;
                if (button.Tag is Edge)
                    if (MessageBox.Show("You cannot restore this operation.\nAre you sure you want to remove this Edge?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        viewModel.graphModel.graph.RemoveEdge(viewModel.CurrentEdgeSelected);
                        CancelSelectionAndResetStatus();
                    }
            }
        }

        private void CanRemoveEdge(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.CurrentEdgeSelected != null;
        }

        private void RemoveVertex(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is Button)
            {
                Button button = e.OriginalSource as Button;
                if (button.Tag is Vertex)
                    if (MessageBox.Show("You cannot restore this operation.\nAre you sure you want to remove this Vertex,\nand all Edges connected to it?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        viewModel.RemoveCurrentSelectedVertex();
                        CancelSelectionAndResetStatus();
                    }
            }
        }

        private void CanRemoveVertex(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.CurrentVertexSelected != null;
        }

        private void ConvertToDirected(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.graphModel.graph.ToDirectedGraph();
        }

        private void CanConvertToDirected(object sender, CanExecuteRoutedEventArgs e)
        {
            if (viewModel == null)
                return;
            e.CanExecute = viewModel.graphModel.graph.CanToDirect;
        }

        private void ConvertToUndirected(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("You cannot restore this operation.\nAll conflicting edges will be merged.\nAre you sure you want to convert the graph to undirected graph?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                viewModel.graphModel.graph.ToUndirectedGraph();
        }

        private void CanConvertToUndirected(object sender, CanExecuteRoutedEventArgs e)
        {
            if (viewModel == null)
                return;
            e.CanExecute = viewModel.graphModel.graph.CanToUndirect;
        }
    }


}
