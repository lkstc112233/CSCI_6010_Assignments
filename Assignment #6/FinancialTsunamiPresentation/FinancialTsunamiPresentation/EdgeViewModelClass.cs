using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FinancialTsunamiPresentation
{
    public class EdgeViewModelClass : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Binding getOnewayBinding(object source, DependencyProperty property, IValueConverter converter)
        {
            if (source is ContentPresenter)
                if ((source as ContentPresenter).Content is Vertex)
                    return getBinding(source, new PropertyPath(property), converter, BindingMode.OneWay, (source as ContentPresenter).Content);
            return getBinding(source, new PropertyPath(property), converter, BindingMode.OneWay);
        }

        private Binding getBinding(object source, PropertyPath path, IValueConverter converter, BindingMode mode, object parameter = null)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = path;
            binding.Converter = converter;
            binding.ConverterParameter = parameter;
            binding.Mode = mode;
            return binding;
        }

        public EdgeViewModelClass(Edge e)
        {
            contentEdge = e;
            IValueConverter converter = new EclipseConverter();
            Binding binding = new Binding();
            binding.Source = start;
            binding.Path = new PropertyPath("color");
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this, ColorProperty, binding);

            BindingOperations.SetBinding(this, X1Property, getOnewayBinding(startPresentser, Canvas.LeftProperty, converter));
            BindingOperations.SetBinding(this, X2Property, getOnewayBinding(endPresentser, Canvas.LeftProperty, converter));
            BindingOperations.SetBinding(this, Y1Property, getOnewayBinding(startPresentser, Canvas.TopProperty, converter));
            BindingOperations.SetBinding(this, Y2Property, getOnewayBinding(endPresentser, Canvas.TopProperty, converter));

            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(getOnewayBinding(startPresentser, Canvas.LeftProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(endPresentser, Canvas.LeftProperty, converter));
            multiBinding.Converter = new LabelPositionConverter();
            BindingOperations.SetBinding(this, LeftEdgeProperty, multiBinding);
            multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(getOnewayBinding(startPresentser, Canvas.TopProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(endPresentser, Canvas.TopProperty, converter));
            multiBinding.Converter = new LabelPositionConverter();
            BindingOperations.SetBinding(this, TopEdgeProperty, multiBinding);
            multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(getOnewayBinding(startPresentser, Canvas.LeftProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(startPresentser, Canvas.TopProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(endPresentser, Canvas.LeftProperty, converter));
            multiBinding.Bindings.Add(getOnewayBinding(endPresentser, Canvas.TopProperty, converter));
            multiBinding.Converter = new AngleConverter();
            BindingOperations.SetBinding(this, RotatingAngleProperty, multiBinding);
        }

        private Edge contentEdge;
        public Edge edge => contentEdge;
        public Vertex start => edge.start;
        public Vertex end => edge.end;
        public double X1 { get { return (double)GetValue(X1Property); } set { SetValue(X1Property, value); } }
        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(EdgeViewModelClass));
        public double Y1 { get { return (double)GetValue(Y1Property); } set { SetValue(Y1Property, value); } }
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(EdgeViewModelClass));
        public double X2 { get { return (double)GetValue(X2Property); } set { SetValue(X2Property, value); } }
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(EdgeViewModelClass));
        public double Y2 { get { return (double)GetValue(Y2Property); } set { SetValue(Y2Property, value); } }
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(EdgeViewModelClass));
        public double RotatingAngle { get { return (double)GetValue(RotatingAngleProperty); } set { SetValue(RotatingAngleProperty, value); } }
        public static readonly DependencyProperty RotatingAngleProperty = DependencyProperty.Register("RotatingAngle", typeof(double), typeof(EdgeViewModelClass));
        public double LeftEdgeHere { get { return (double)GetValue(LeftEdgeProperty); } set { SetValue(LeftEdgeProperty, value); } }
        public static readonly DependencyProperty LeftEdgeProperty = DependencyProperty.Register("LeftEdgeHere", typeof(double), typeof(EdgeViewModelClass));
        public double TopEdgeHere { get { return (double)GetValue(TopEdgeProperty); } set { SetValue(TopEdgeProperty, value); } }
        public static readonly DependencyProperty TopEdgeProperty = DependencyProperty.Register("TopEdgeHere", typeof(double), typeof(EdgeViewModelClass));
        public Color color { get { return (Color)GetValue(ColorProperty); } set { SetValue(ColorProperty, value); } }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("color", typeof(Color), typeof(EdgeViewModelClass));

        public ContentPresenter startPresentser => ViewModelVertexEdge.findVertexPresenter(start);
        public ContentPresenter endPresentser => ViewModelVertexEdge.findVertexPresenter(end);

        class LabelPositionConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                return ((double)values[0] * 1.0 / 3.0) + ((double)values[1] * 2.0 / 3.0);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        class AngleConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                double xs = (double)values[0];
                double ys = (double)values[1];
                double xe = (double)values[2];
                double ye = (double)values[3];
                double dx = xe - xs;
                double dy = ys - ye;
                double d = Math.Pow((Math.Pow(dx, 2) + Math.Pow(dy, 2)), 0.5);
                if (dy > 0)
                    return Math.Asin(dx / d) / Math.PI * 180;
                if (dx > 0)
                    return Math.Acos(dy / d) / Math.PI * 180;
                return -Math.Acos(dy / d) / Math.PI * 180;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}