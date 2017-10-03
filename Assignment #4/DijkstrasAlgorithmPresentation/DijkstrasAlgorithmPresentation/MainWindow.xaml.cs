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
        List<Vertex> vertexes;

        Nullable<Point> dragStart = null;

        
        Vertex m_vertexSelected
        {
            get
            {
                return (Vertex)GetValue(CurrentVertexSelected);
            }
            set
            {
                SetValue(CurrentVertexSelected, value);
            }
        }
        public static readonly DependencyProperty CurrentVertexSelected = DependencyProperty.Register("CurrentVertexSelected", typeof(Vertex),typeof(MainWindow),new PropertyMetadata(null));


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            moveStart = (varMoved, args) =>
            {
                var element = varMoved as UIElement;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            moveEnd = (varMoved, args) =>
            {
                var element = varMoved as UIElement;
                dragStart = null;
                element.ReleaseMouseCapture();
            };
            moving = (varMoved, args) =>
            {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = (UIElement)varMoved;
                    var p2 = args.GetPosition(monitor);
                    if (p2.X - dragStart.Value.X > 0)
                        Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                    else
                        Canvas.SetLeft(element, 0);
                    if (p2.Y - dragStart.Value.Y > 0)
                        Canvas.SetTop(element, p2.Y - dragStart.Value.Y);
                    else
                        Canvas.SetTop(element, 0);
                }
            };

            selectPresenter = (varMoved, args) =>
            {
                var presenter = varMoved as ContentPresenter;
                if (presenter.Content is Vertex)
                    m_vertexSelected = presenter.Content as Vertex;
                args.Handled = true;
            };

        }

        private void Add_Circle_Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void Set_Vertex_try(object sender, RoutedEventArgs e)
        {
            m_vertexSelected = new Vertex();
            m_vertexSelected.color = Colors.Green;
            m_vertexSelected = m_vertexSelected;
        }

        private void Make_Vertex_Null(object sender, RoutedEventArgs e)
        {
            m_vertexSelected = null;
        }

        MouseButtonEventHandler moveStart;
        MouseButtonEventHandler moveEnd;
        MouseEventHandler moving;
        
        MouseButtonEventHandler selectPresenter;
        MouseButtonEventHandler vertexMouseUp;
        MouseEventHandler vertexMouseMoving;

        private void Add_Vertex(object sender, RoutedEventArgs e)
        {
            var rd = new ResourceDictionary();
            rd.Source = new Uri("ControlPanelDisplayDictionary.xaml", UriKind.RelativeOrAbsolute);
            if (vertexes == null)
                vertexes = new List<Vertex>();
            vertexes.Add(new Vertex());
            vertexes.Last().id = vertexes.Count;
            m_vertexSelected = vertexes.Last();
            var cont = new ContentPresenter();
            cont.ContentTemplate = (DataTemplate)rd["VertexNode"];
            cont.Content = vertexes.Last();

            cont.MouseDown += moveStart;
            cont.MouseDown += selectPresenter;
          //  cont.MouseUp += vertexMouseUp;
            cont.MouseUp += moveEnd;
            cont.MouseMove += moving;

            monitor.Children.Add(cont);
        }

        private void CancelSelection(object sender, MouseButtonEventArgs e)
        {
            m_vertexSelected = null;
        }
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
