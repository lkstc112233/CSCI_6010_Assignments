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
    public class Edge : INotifyPropertyChanged
    {
        public Vertex start { get; set; }
        public Vertex end { get; set; }
        public int id { get; set; }
        private bool m_oneway = true;

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

        private Color m_color = Colors.Blue;

        public Color color
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
                NotifyPropertyChanged("color");
            }
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
