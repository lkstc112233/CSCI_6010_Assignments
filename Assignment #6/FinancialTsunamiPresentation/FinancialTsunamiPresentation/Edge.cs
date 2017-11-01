using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinancialTsunamiPresentation
{
    public class Edge : INotifyPropertyChanged
    {
        private Vertex m_vertexStart = null;
        public Vertex start
        {
            get => m_vertexStart;
            set {
                if (m_vertexStart != null)
                    m_vertexStart.PropertyChanged -= M_vertexStart_PropertyChanged;
                m_vertexStart = value;
                if (m_vertexStart != null)
                    m_vertexStart.PropertyChanged += M_vertexStart_PropertyChanged;
            }
        }

        private void M_vertexStart_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("colorReal"))
                NotifyPropertyChanged("color");
        }

        public Vertex end { get; set; }
        public int id { get; set; }
        
        private bool m_selected = false;


        public Color color
        {
            get
            {
                if (m_selected)
                    return Colors.Cyan;
                return m_vertexStart != null ? m_vertexStart.colorReal : Colors.Gray;
            }
        }

        public void SelectEdge()
        {
            m_selected = true;
            NotifyPropertyChanged("color");
        }

        public void NotSelectEdge()
        {
            m_selected = false;
            NotifyPropertyChanged("color");
        }

        private double m_weight;
        public double weight
        {
            get
            {
                return m_weight;
            }
            set
            {
                m_weight = value;
                NotifyPropertyChanged("weight");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
