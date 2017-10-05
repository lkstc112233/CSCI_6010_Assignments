using System.ComponentModel;

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


        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}