using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DijkstrasAlgorithmPresentation
{
    public class Vertex : INotifyPropertyChanged
    {
        private Color m_color;
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

        private double m_radius = 20;
        public double radius
        {
            get
            {
                return m_radius;
            }
            set
            {
                double d = m_radius - value;
                m_radius = value;
                Canvas.SetLeft(ViewModelVertexEdge.findVertexPresenter(this), Canvas.GetLeft(ViewModelVertexEdge.findVertexPresenter(this)) + d / 2);
                Canvas.SetTop(ViewModelVertexEdge.findVertexPresenter(this), Canvas.GetTop(ViewModelVertexEdge.findVertexPresenter(this)) + d / 2);
                NotifyPropertyChanged("radius");
            }
        }

        public int id { get; set; }

        public Vertex()
        {
            color = Colors.Red;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    class ColorSelectionToIsCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals((Color)ColorConverter.ConvertFromString(parameter as string));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? (Color)ColorConverter.ConvertFromString(parameter as string) : Binding.DoNothing;
        }
    }

    class RadiusSelectionToIsCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if (Double.TryParse(parameter as string, out result))
                return result == (double)value;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if (Double.TryParse(parameter as string, out result))
                if (value.Equals(true))
                    return result;
            return Binding.DoNothing;
        }
    }
}
