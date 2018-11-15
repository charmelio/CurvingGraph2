using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CurvingGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Serializable]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AttachDrawingHandlers();
            CreateEmptyCanvas();
            PaintCurvingGraph(this, null);
        }

        public void PaintCurvingGraph(object sender, EventArgs e)
        {
            GraphingCanvas.Children.Clear();

            for (var i = 0; i < Graph.LineCount; i++)
            {
                Debug.Assert(Graph.LineCount > 0);

                var x = (GraphingCanvas.Width / Graph.LineCount) * i;
                var y = GraphingCanvas.Height - (GraphingCanvas.Height / Graph.LineCount) * i;

                GraphingCanvas.Children.Add(new Line()
                {
                    Stroke = Brushes.Black,
                    X1 = x,
                    Y1 = y,
                    X2 = GraphingCanvas.Width,
                    Y2 = GraphingCanvas.Height,
                    StrokeThickness = 1d,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom
                });

                GraphingCanvas.Children.Add(new Path()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1d,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Data = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                IsClosed = false,
                                StartPoint = new Point(0, GraphingCanvas.Height),
                                Segments = new PathSegmentCollection()
                                {
                                    new ArcSegment()
                                    {
                                        IsLargeArc = false,
                                        Point = new Point(GraphingCanvas.Width, 0),
                                        RotationAngle = Math.Atan2(y, x) * i + i,
                                        Size = new Size(GraphingCanvas.Width, GraphingCanvas.Height),
                                        SweepDirection = SweepDirection.Counterclockwise
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }

        private void AttachDrawingHandlers()
        {
            GraphingCanvas.MouseDown += new MouseButtonEventHandler(PaintCurvingGraph);
            GraphingCanvas.MouseWheel += new MouseWheelEventHandler(PaintCurvingGraph);
            WindowMain.SizeChanged += new SizeChangedEventHandler(PaintCurvingGraph);
        }

        private void CreateEmptyCanvas()
        {
            GraphingCanvas.Children.Clear();
            GraphingCanvas.Background = new SolidColorBrush(Colors.White);
        }

        private void GraphingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Graph.LineCount = 50;
            }
        }

        private void GraphingCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Graph.LineCount++;
            }

            if (e.Delta < 0)
            {
                if (Graph.LineCount <= 0)
                {
                    Graph.LineCount = 0;
                    return;
                }

                Graph.LineCount--;
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
            }
        }

        private void WindowMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GraphingCanvas.Width = ((Window)sender).Width - 28;
            GraphingCanvas.Height = ((Window)sender).Height - 51;
        }
    }
}