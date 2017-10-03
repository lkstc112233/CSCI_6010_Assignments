using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DijkstrasAlgorithmPresentation
{
    enum ColorSelection
    {
        RED,
        GREEN,
        BLUE,
        BLACK,
        WHITE,
        CYAN,
        BROWN,
        PINK,
        YELLOW,
        AQUA,
        GREY,
    }
    class Vertex : INotifyPropertyChanged
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

        public int id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
}
