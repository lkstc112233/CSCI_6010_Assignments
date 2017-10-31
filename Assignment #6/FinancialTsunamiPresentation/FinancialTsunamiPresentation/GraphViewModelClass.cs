using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace FinancialTsunamiPresentation
{
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
                case NotifyCollectionChangedAction.Reset:
                    modelPair.Clear();
                    edgeModels.Clear();
                    break;
            }
        }

        private Graph contentGraph;
        public Graph graph => contentGraph;

    }
}