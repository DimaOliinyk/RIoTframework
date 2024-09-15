using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace CourseWorkUI;

public partial class MainPage : ContentPage
{
    static private double _X;
    static private double _Y;
    static private double _tileSize = 100;

    public MainPage()
    {
        InitializeComponent();
    }

    internal class RectangleDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.DarkBlue;
            canvas.StrokeSize = 1;
            canvas.FillRoundedRectangle((float)_X, (float)_Y, (float)_tileSize - 2, (float)_tileSize - 2, 10);
        }
    }

    private void MCanvas_Clicked(object sender, EventArgs e)
    {
        _X = ((TappedEventArgs)e).GetPosition(this)!.Value.X;
        _Y = ((TappedEventArgs)e).GetPosition(this)!.Value.Y;
        _X = _X - _X % _tileSize;
        _Y = _Y - _Y % _tileSize;

        var graphicsView = new GraphicsView();
        graphicsView.HeightRequest = _tileSize*5;
        graphicsView.WidthRequest = _tileSize*4;
        graphicsView.Drawable = new RectangleDrawable();
        Layout.Children.Add(graphicsView);
    }
}

