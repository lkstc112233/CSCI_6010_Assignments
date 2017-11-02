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
using System.Windows.Threading;

namespace FinancialTsunamiPresentation
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModelVertexEdge viewModel;

        Nullable<Point> dragStart = null;
        Financial_Tsunami_Followup_Window FollowupWindow;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModelVertexEdge();
            DataContext = viewModel;

            ViewModelVertexEdge.graphControl = monitor;
            FollowupWindow = new Financial_Tsunami_Followup_Window();
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
            public int startid = 0;
            public int endid = 0;
            public double weight = 0;

            public EdgeEntry(int startId, int endid, double weight)
            {
                this.startid = startId;
                this.endid = endid;
                this.weight = weight < 0 ? 0 : weight;
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
            int number = 0;
            double limit = 0;
            try
            {
                if (!int.TryParse(data[0], out number))
                    return;
                if (!double.TryParse(data[1], out limit))
                    return;
                int i = 2;
                int startid = 0;
                double balance = -1;
                while (i < data.Count && double.TryParse(data[i], out balance))
                {
                    i += 1;
                    int dataCount = 0;
                    if (!int.TryParse(data[i], out dataCount))
                        break;
                    i += 1;
                    for (int j = 0; j < dataCount; ++j)
                    {
                        int endid = 0;
                        double weight = 0;
                        if (!int.TryParse(data[i], out endid))
                            break;
                        i += 1;
                        if (!double.TryParse(data[i], out weight))
                            break;
                        i += 1;
                        edgeEntries.Add(new EdgeEntry(startid, endid, weight));
                    }
                    startid += 1;
                }
            }
            catch (IndexOutOfRangeException) { }
            if (edgeEntries.Count() == 0)
                return;
            int maxVertexId = number;

            ResetPresentationThingsAndGraph();

            List<Vertex> temp = new List<Vertex>();
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
            CommandManager.InvalidateRequerySuggested();
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
            CommandManager.InvalidateRequerySuggested();
        }

        private void CanRemoveEdge(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.CurrentEdgeSelected != null && viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph;
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
            CommandManager.InvalidateRequerySuggested();
        }

        private void CanRemoveVertex(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.CurrentVertexSelected != null && viewModel.CurrentProgramStatus == ProgramStatus.BuildingGraph;
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
            CommandManager.InvalidateRequerySuggested();
        }

        private void FileDragEnterProcess(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void parentWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void LoadFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadFile(ofd.FileName);
        }

        private void ResetPresentationThingsAndGraph()
        {
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
                dispatcherTimer = null;
            }
            viewModel.EndPresentation();
            BeginTheShowButton.Content = "Begin Automatic Presentation!";
            BeginButton.Content = "Begin The Presentation!";
            viewModel.ClearGraph();
            CancelSelectionAndResetStatus();
        }

        private void ResetGraph(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "You cannot restore this operation.\nAll Vertexex and Edges will be removed.\nAre you sure you want to reset the graph?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes ||
                MessageBox.Show(
                    "Seriously, you can't restore this operation.\nAll information you have so far will be ERASED.\nAre you sure you want to remove all process you have?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;
            ResetPresentationThingsAndGraph();
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            Vertex LatestVertex = CreateVertex();
            viewModel.SelectVertex(LatestVertex);
        }

        private void SelectStartingPoint(object sender, RoutedEventArgs e)
        {
            viewModel.CurrentStatus = SelectStatus.SelectAStartingVertex;
        }

        private void SelectEndPoint(object sender, RoutedEventArgs e)
        {
            if (viewModel.VertexEnd == null)
            {
                SelectEndPointButton.Content = "Deselect End Vertex"; 
                viewModel.CurrentStatus = SelectStatus.SelectAnEndVertex;
            }
            else
            {
                SelectEndPointButton.Content = "Selecting End Vertex"; 
                viewModel.CancelSelectEndVertex();
            }
        }

        private void OneStep(object sender, RoutedEventArgs e)
        {
            viewModel.AlgorithmData.OneStep();
        }

        private void SolveInAFlash(object sender, RoutedEventArgs e)
        {
            while (viewModel.AlgorithmData.OneStep()) ;
        }
        
        private void BeginPresentation(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentProgramStatus != ProgramStatus.Presenting)
            {
                BeginButton.Content = "End the presentation";
                viewModel.BeginPresentation();
            }
            else
            {
                BeginButton.Content = "Begin the presentation!";
                viewModel.EndPresentation();
            }
        }

        DispatcherTimer dispatcherTimer = null;

        private void SolvePresentation(object sender, RoutedEventArgs e)
        {
            if (dispatcherTimer == null)
            {
                BeginTheShowButton.Content = "End Automatic Presentation";

                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += (Sender, args) =>
                {
                    if (!viewModel.AlgorithmData.OneStep())
                    {
                        dispatcherTimer.Stop();
                        BeginTheShowButton.Content = "Begin Automatic Presentation!";
                        dispatcherTimer = null;
                    }
                };
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            }
            else
            {
                BeginTheShowButton.Content = "Begin Automatic Presentation!";
                dispatcherTimer.Stop();
                dispatcherTimer = null;
            }
        }
    }
}
