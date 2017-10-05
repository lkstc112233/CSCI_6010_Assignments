using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DijkstrasAlgorithmPresentation
{
    class ValueConverterGroups : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class IsNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class VertexToContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Vertex v = value as Vertex;
                UIElement e = ViewModelVertexEdge.vertexPresenterDictionary[v];
                if (e == null)
                    return Binding.DoNothing;
                return e;
            }
            catch (InvalidCastException)
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class AverangeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0;
            foreach (object o in values)
            {
                result += (double)o;
            }
            return result / values.Count();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class EclipseConverter : IValueConverter
    {
        public double Radius { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value + 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - 10;
        }
    }

    class UIElementToXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            UIElement elem = value as UIElement;
            return Canvas.GetLeft(elem);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class UIElementToYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            UIElement elem = value as UIElement;
            return Canvas.GetTop(elem);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

