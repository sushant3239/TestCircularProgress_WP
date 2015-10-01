using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TestProgressBar.CustomControls
{
    public sealed partial class CircularProgressBar : UserControl
    {
        public CircularProgressBar()
        {
            this.InitializeComponent();
        }

        public Brush FontBrush
        {
            get { return (Brush)GetValue(FontBrushProperty); }
            set { SetValue(FontBrushProperty, value); }
        }

        public static readonly DependencyProperty FontBrushProperty =
            DependencyProperty.Register("FontBrush",
                typeof(Brush),
                typeof(CircularProgressBar),
                null);


        public Brush BgBrush
        {
            get { return (Brush)GetValue(BgBrushProperty); }
            set { SetValue(BgBrushProperty, value); }
        }

        public static readonly DependencyProperty BgBrushProperty =
            DependencyProperty.Register("BgBrush",
                typeof(Brush),
                typeof(CircularProgressBar),
                null);


        public Brush HoleBrush
        {
            get { return (Brush)GetValue(HoleBrushProperty); }
            set { SetValue(HoleBrushProperty, value); }
        }

        public static readonly DependencyProperty HoleBrushProperty =
            DependencyProperty.Register("HoleBrush",
                typeof(Brush),
                typeof(CircularProgressBar),
                null);


        public Brush ForegroundBrush
        {
            get { return (Brush)GetValue(ForegroundBrushProperty); }
            set { SetValue(ForegroundBrushProperty, value); }
        }

        public static readonly DependencyProperty ForegroundBrushProperty =
            DependencyProperty.Register("ForegroundBrush",
                typeof(Brush),
                typeof(CircularProgressBar),
                null);


        public double CurrentProgress
        {
            get { return (double)GetValue(CurrentProgressProperty); }
            set { SetValue(CurrentProgressProperty, value); }
        }

        public static readonly DependencyProperty CurrentProgressProperty =
            DependencyProperty.Register("CurrentProgress",
                typeof(double),
                typeof(CircularProgressBar),new PropertyMetadata(default(double),CurrentProgressChanged));

        private static void CurrentProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as CircularProgressBar;
            if(obj != null)
            {
                obj.DrawSector((double)e.NewValue);
            }
        }

        private void DrawSector(double progress)
        {
            path.Data = null;
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();

            if (progress == 0)
            {
                return;
            }

            var height = this.ActualHeight;
            var width = this.ActualWidth;
            var radius = height / 2;
            var theta = (360 * progress) - 90;
            var xC = radius;
            var yC = radius;

            if (progress == 1)
            {
                theta += 1;
            }

            var endX = xC + (radius * Math.Cos(theta * 0.0174));
            var endY = yC + (radius * Math.Sin(theta * 0.0174));

            pathFigure.StartPoint = new Point(radius, radius);
            var firstLine = new LineSegment { Point = new Point(radius, 0) };
            pathFigure.Segments.Add(firstLine);

            if (progress > 0.25)
            {
                var firstQuart = new ArcSegment
                {
                    Point = new Point(width, radius),
                    SweepDirection = SweepDirection.Clockwise,
                    Size = new Size(radius, radius)
                };
                pathFigure.Segments.Add(firstQuart);
            }

            if (progress > 0.5)
            {
                var secondQuart = new ArcSegment
                {
                    Point = new Point(radius, height),
                    SweepDirection = SweepDirection.Clockwise,
                    Size = new Size(radius, radius),
                };
                pathFigure.Segments.Add(secondQuart);
            }

            if (progress > 0.75)
            {
                var thirdQuart = new ArcSegment
                {
                    Point = new Point(0, radius),
                    SweepDirection = SweepDirection.Clockwise,
                    Size = new Size(radius, radius),
                };
                pathFigure.Segments.Add(thirdQuart);
            }

            var finalQuart = new ArcSegment
            {
                Point = new Point(endX, endY),
                SweepDirection = SweepDirection.Clockwise,
                Size = new Size(radius, radius)
            };
            pathFigure.Segments.Add(finalQuart);

            var lastLine = new LineSegment { Point = new Point(radius, radius) };
            pathFigure.Segments.Add(lastLine);

            pathGeometry.Figures.Add(pathFigure);
            path.Data = pathGeometry;
        }

        private void CircularProgressLoaded(object sender, RoutedEventArgs e)
        {
            DrawSector(CurrentProgress);
        }
    }
}
