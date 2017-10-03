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
        private ColorSelection m_m_color;
        public ColorSelection m_color
        {
            get
            {
                return m_m_color;
            }
            set
            {
                m_m_color = value;
                NotifyPropertyChanged("m_color");
            }
        }

        public int id { get; set; }
        public Color color { get
            {
                switch(m_color)
                {
                    default:
                    case ColorSelection.RED:
                        return Colors.Red;
                    case ColorSelection.GREEN:
                        return Colors.Green;
                    case ColorSelection.BLUE:
                        return Colors.Blue;
                    case ColorSelection.BLACK:
                        return Colors.Black;
                    case ColorSelection.WHITE:
                        return Colors.White;
                    case ColorSelection.CYAN:
                        return Colors.Cyan;
                    case ColorSelection.BROWN:
                        return Colors.Brown;
                    case ColorSelection.PINK:
                        return Colors.Pink;
                    case ColorSelection.YELLOW:
                        return Colors.Yellow;
                    case ColorSelection.AQUA:
                        return Colors.Aqua;
                }
            }
        }

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
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
