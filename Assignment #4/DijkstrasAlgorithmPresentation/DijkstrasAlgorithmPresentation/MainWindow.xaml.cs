using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DijkstrasAlgorithmPresentation
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ellipse> circles;

        Nullable<Point> dragStart = null;


        public MainWindow()
        {
            InitializeComponent();
        }



        private void Add_Circle_Button_Click(object sender, RoutedEventArgs e)
        {
            MouseButtonEventHandler moveStart = (varMoved, args) =>
            {
                var element = varMoved as UIElement;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            MouseButtonEventHandler moveEnd = (varMoved, args) =>
            {
                var element = varMoved as UIElement;
                dragStart = null;
                element.ReleaseMouseCapture();
            };
            MouseEventHandler moving = (varMoved, args) =>
            {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = (UIElement)varMoved;
                    var p2 = args.GetPosition(monitor);
                    Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                    Canvas.SetTop(element, p2.Y - dragStart.Value.Y);

                }
            };

            if (circles == null)
                circles = new List<Ellipse>();
            Ellipse newOne = new Ellipse() { Fill = Brushes.Red, Width = 20, Height = 20 };

            // Enable drag.
            newOne.MouseDown += moveStart;
            newOne.MouseUp += moveEnd;
            newOne.MouseMove += moving;

            monitor.Children.Add(newOne);
            Canvas.SetLeft(newOne, 10);
            Canvas.SetTop(newOne, 10);
            Canvas.SetZIndex(newOne, 10);

            if (circles.Count == 0)
            {
                circles.Add(newOne);
                return;
            }

            Ellipse last = circles.Last();
            circles.Add(newOne);
            Line connection = new Line() { Stroke = Brushes.Cyan, StrokeThickness = 2 };
            Binding binding = new Binding();
            binding.Source = last;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.X1Property, binding);
            binding = new Binding();
            binding.Source = newOne;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.X2Property, binding);
            binding = new Binding();
            binding.Source = last;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.Y1Property, binding);
            binding = new Binding();
            binding.Source = newOne;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            binding.Converter = new EclipseConverter();
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(connection, Line.Y2Property, binding);

            monitor.Children.Add(connection);
        }

        class EclipseConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (double)value + 10;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (double)value - 10;
            }
        }
    }
}
