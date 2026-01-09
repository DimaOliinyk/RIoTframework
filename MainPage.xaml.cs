using CourseWorkUI.UI;
using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls;

namespace CourseWorkUI;

public partial class MainPage : ContentPage
{
    static private Position _pos = new Position(0,0);
    static private double _tileSize = 100;
    static private TileGrid _grid = new TileGrid(DeviceDisplay.Current.MainDisplayInfo.Height - 2 /6,
                                                 DeviceDisplay.Current.MainDisplayInfo.Width - 2 / 6);

    public MainPage()
    {
        InitializeComponent();

        Layout.Drawable = new RectangleDrawable();
        
        _tileSize = (DeviceDisplay.Current.MainDisplayInfo.Width / 6);
    }

    internal class RectangleDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            _grid.RedrawOnCanvas(canvas, dirtyRect);
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
            _grid.RemoveTile(_grid.GetTile(tile.Position));
            Layout.Invalidate();
            return;
        }
        _pos = pos;

        _grid.AddTile(tile);
        Layout.Invalidate();
    }
}

