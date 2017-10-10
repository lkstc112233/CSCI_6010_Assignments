using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DijkstrasAlgorithmPresentation
{
    public enum EdgeType
    {
        Unselected,
        UnscannedEdge,
        ListedEdge,
        ScanningEdge,
        ScannedEdge,
    }

    public class Edge : INotifyPropertyChanged
    {
        public Vertex start { get; set; }
        public Vertex end { get; set; }
        public int id { get; set; }
        private bool m_oneway = true;

        private EdgeType m_type = EdgeType.Unselected;
        private bool m_selected = false;

        public bool oneway
        {
            get
            {
                return m_oneway;
            }
            set
            {
                m_oneway = value;
                NotifyPropertyChanged("oneway");
            }
        }
        
        public Color color
        {
            get
            {
                if (m_selected)
                    return Colors.Cyan;
                switch (m_type)
                {
                    case EdgeType.ScannedEdge:
                        return Color.FromRgb(0xB7, 0xB8, 0xB6);
                    case EdgeType.ScanningEdge:
                        return Color.FromRgb(0x34, 0x67, 0x5C);
                    case EdgeType.ListedEdge:
                        return Color.FromRgb(0xB3, 0xC1, 0x00);
                    case EdgeType.Unselected:
                    case EdgeType.UnscannedEdge:
                    default:
                        return Color.FromRgb(0x4C, 0xB5, 0xF5);
                }
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

        public void SetType(EdgeType type)
        {
            m_type = type;
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
