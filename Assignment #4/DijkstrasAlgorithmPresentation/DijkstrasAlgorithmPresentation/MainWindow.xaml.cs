using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        Dijkstra_s_Algorithm_Followup FollowupWindow;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModelVertexEdge();
            DataContext = viewModel;

            ViewModelVertexEdge.graphControl = monitor;
            FollowupWindow = new Dijkstra_s_Algorithm_Followup();
            FollowupWindow.viewModel = viewModel;
            FollowupWindow.DataContext = viewModel;

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

                switch (viewModel.CurrentStatus)
                {
                    case SelectStatus.SelectAnElement:
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
                        break;
                    case SelectStatus.EdgeBuilding:
                        if (presenter.Content is Vertex && presenter.Content != viewModel.CurrentVertexSelected)
                        {
                            Edge e = AddEdge(viewModel.CurrentVertexSelected, presenter.Content as Vertex);
                            CancelSelectionAndResetStatus();
                            if (e != null)
                                viewModel.SelectEdge(e);
                            args.Handled = true;
                        }
                        else
                            CancelSelectionAndResetStatus(); 
                        break;
                    case SelectStatus.SelectAStartingVertex:
                        if (presenter.Content is Vertex)
                        {
                            CancelSelectionAndResetStatus();
                            viewModel.SelectStartVertex(presenter.Content as Vertex);
                            args.Handled = true;
                        }
                        else
                            CancelSelectionAndResetStatus();
                        break;
                    case SelectStatus.SelectAnEndVertex:
                        if (presenter.Content is Vertex)
                        {
                            CancelSelectionAndResetStatus();
                            viewModel.SelectEndVertex(presenter.Content as Vertex);
                            args.Handled = true;
                        }
                        else
                            CancelSelectionAndResetStatus();
                        break;
                    default:
                        CancelSelectionAndResetStatus();
                        break;
                }
            };
            

        }

        MouseButtonEventHandler moveStart;
        MouseButtonEventHandler moveEnd;
        MouseEventHandler moving;
        
        MouseButtonEventHandler selectPresenter;

        private class EdgeEntry
        {
            public int edgeid = 0;
            public int startid = 0;
            public int endid = 0;
            public double weight = 0;

            public EdgeEntry(int edgeid, int startId, int endid, double weight)
            {
                this.edgeid = edgeid;
                this.startid = startId;
                this.endid = endid;
                this.weight = weight;
            }
        }

        private void LoadFile(string fileName)
        {
            if (!File.Exists(fileName))
                return;
            var reader = File.OpenText(fileName);
            string s;
            try
            {
                s = reader.ReadToEnd();
            }
            catch (IOException)
            {
                return;
            }
            ProcessTextLoaded(s);
        }

        private void ProcessTextLoaded(string s)
        {
            string[] dataT = s.Split(null);
            List<string> data = dataT.ToList();
            data.RemoveAll(str => str.Equals(""));
            List<EdgeEntry> edgeEntries = new List<EdgeEntry>();
            try
            {
                for (int i = 0; i < data.Count(); ++i)
                {
                    int edgeid = 0;
                    int startid = 0;
                    int endid = 0;
                    double weight = 0;
                    if (!int.TryParse(data[i], out edgeid))
                        break;
                    i += 1;
                    if (!int.TryParse(data[i], out startid))
                        break;
                    i += 1;
                    if (!int.TryParse(data[i], out endid))
                        break;
                    i += 1;
                    if (!double.TryParse(data[i], out weight))
                        break;
                    edgeEntries.Add(new EdgeEntry(edgeid, startid, endid, weight));
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
            if (edgeEntries.Count() == 0)
                return;
            int maxVertexId = -1;
            foreach (EdgeEntry e in edgeEntries)
            {
                if (e.startid > maxVertexId)
                    maxVertexId = e.startid;
                if (e.endid > maxVertexId)
                    maxVertexId = e.endid;
            }

            viewModel.ClearGraph();

            List<Vertex> temp = new List<Vertex>();
            temp.Add(null);
            for (int i = 0; i < maxVertexId; ++i)
                temp.Add(CreateVertex());
            foreach (EdgeEntry e in edgeEntries)
                AddEdge(temp[e.startid], temp[e.endid]).weight = e.weight;
            viewModel.RearrangeVertexes();
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
            e.CanExecute = viewModel.CurrentStatus == SelectStatus.SelectAnElement && viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph;
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
            e.CanExecute = viewModel.CurrentEdgeSelected != null && viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph; ;
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
            e.CanExecute = viewModel.CurrentVertexSelected != null && viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph; ;
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
            {
                viewModel.graphModel.graph.ToUndirectedGraph();
                CancelSelectionAndResetStatus();
            }
        }

        private void CanConvertToUndirected(object sender, CanExecuteRoutedEventArgs e)
        {
            if (viewModel == null)
                return;
            e.CanExecute = viewModel.graphModel.graph.CanToUndirect;
        }

        private void AddVertex(object sender, ExecutedRoutedEventArgs e)
        {
            Vertex LatestVertex = CreateVertex();
            viewModel.SelectVertex(LatestVertex);
        }

        private void CanAddVertex(object sender, CanExecuteRoutedEventArgs e)
        {
            if (viewModel == null)
                return;
            e.CanExecute = viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph;
        }

        private void LoadFile(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadFile(ofd.FileName);
        }

        private void CanLoadFile(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ResetGraph(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("You cannot restore this operation.\nAll Vertexex and Edges will be removed.\nAre you sure you want to reset the graph?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes || MessageBox.Show(
                    "Seriously, you can't restore this operation.\nAll information you have so far will be ERASED.\nAre you sure you want to remove all process you have?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;
            viewModel.ClearGraph();
            CancelSelectionAndResetStatus();
        }

        private void CanResetGraph(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FileDropProcess(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadFile(files[0]);
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.Data.GetData(DataFormats.Text);
                ProcessTextLoaded(text);
            }
        }

        private void FileDragEnterProcess(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetDataPresent(DataFormats.FileDrop)|| e.Data.GetDataPresent(DataFormats.Text)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void SelectStartingPoint(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.CurrentStatus=SelectStatus.SelectAStartingVertex;
        }

        private void CanSelectStartingPoint(object sender, CanExecuteRoutedEventArgs e)
        {
            if (viewModel == null)
                return;
            e.CanExecute = viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph;
        }

        private void SelectEndingPoint(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.CurrentStatus = SelectStatus.SelectAnEndVertex;
        }

        private void CanSelectEndingPoint(object sender, CanExecuteRoutedEventArgs e)
        {
            if (viewModel == null)
                return;
            e.CanExecute = viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph;
        }
    }
}
