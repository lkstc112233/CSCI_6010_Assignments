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

namespace FinancialTsunamiPresentation
{
    public class Vertex : INotifyPropertyChanged
    {
        private bool m_selected = false;
        private bool m_building = false;
        private bool m_safe = true;
        public Color color
        {
            get
            {
                if (m_selected)
                    if (m_building)
                        return Colors.Green;
                    else
                        return Colors.Cyan;
                return colorReal;
            }
        }
        public Color colorReal
        {
            get
            {
                if (m_safe)
                    return Color.FromRgb(0x8E, 0xBA, 0x43);
                else
                    return Color.FromRgb(0xFF, 0x42, 0x0E);
            }
        }
        public void BuildEdge()
        {
            m_selected = true;
            m_building = true;
            NotifyPropertyChanged("color");
        }
        public void Select()
        {
            m_selected = true;
            m_building = false;
            NotifyPropertyChanged("color");
        }
        public void NotSelect()
        {
            m_selected = false;
            m_building = false;
            NotifyPropertyChanged("color");
        }
        
        public bool safe
        {
            get
            {
                return m_safe;
            }
            set
            {
                m_safe = value;
                NotifyPropertyChanged("color");
                NotifyPropertyChanged("colorReal");
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

        private double m_balance = 0;
        public double balance
        {
            get
            {
                return m_balance;
            }
            set
            {
                m_balance = value;
                NotifyPropertyChanged("balance");
            }
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
