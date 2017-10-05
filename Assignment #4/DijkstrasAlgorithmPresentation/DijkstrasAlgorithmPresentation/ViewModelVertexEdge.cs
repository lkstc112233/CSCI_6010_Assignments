using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

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
}