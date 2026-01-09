using CourseWorkUI.UI;
using CourseWorkUI.Utilities;

namespace CourseWorkUI;

public partial class MainPage : ContentPage
{
    static private Position _pos = new Position(0,0);
    static private double _tileSize = 100;
    static private TileGrid _grid = new TileGrid(_tileSize*5, _tileSize*4);

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
            canvas.FillRoundedRectangle((float)_pos.X, (float)_pos.Y, (float)_tileSize - 2, (float)_tileSize - 2, 10);
        }
    }

    private void MCanvas_Clicked(object sender, EventArgs e)
    {
        var pos = new Position(
            ((TappedEventArgs)e).GetPosition(this)!.Value.X,
            ((TappedEventArgs)e).GetPosition(this)!.Value.Y);
        pos.Round(_tileSize);

        var tile = new Tile(pos, _tileSize, _tileSize);

        //TODO: Save result of CanAddTile to save on time
        if(_grid.CanAddTile(tile) == ETileGrid.OUT_OF_BOUNDS) 
        {
            return;
        }

        if (_grid.CanAddTile(tile) == ETileGrid.FALSE) 
        {
            _grid.GetTile(tile.Position);
            Layout.Children.Clear();
            
            //TODO: Layout.Children.Remove();

            _grid.Clear();
            return;
        }
        _pos = pos;
        _grid.AddTile(tile);
        // TODO: Save GraphicsView to list in the Grid class and remove them
        // When clicked
        var graphicsView = new GraphicsView();
        graphicsView.HeightRequest = _tileSize*4;
        graphicsView.WidthRequest = _tileSize*5;
        graphicsView.Drawable = new RectangleDrawable();
        Layout.Children.Add(graphicsView);
    }
}

