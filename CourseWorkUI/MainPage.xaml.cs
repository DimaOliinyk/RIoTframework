using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls;
#if WINDOWS
using Windows.Storage.Provider;
#endif

namespace CourseWorkUI;

public partial class MainPage : ContentPage
{
    public static Position Position { get; set; } = new Position(0, 0);
    private static double _tileSize;
    private static TileGrid _grid = new TileGrid(DeviceDisplay.Current.MainDisplayInfo.Width / 3, //360
                                                 Math.Round(
                                                     DeviceDisplay.Current.MainDisplayInfo.Height /
                                                     (DeviceDisplay.Current.MainDisplayInfo.Width)) *
                                                     DeviceDisplay.Current.MainDisplayInfo.Width / 3 - 60);   //780

    public static string? TileType { get; set; } = null;

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
            if (AppState.IsRunning == false)
                _grid.DrawPoints(canvas, dirtyRect, _tileSize);

            _grid.RedrawOnCanvas(canvas, dirtyRect);
        }
    }

    private void MCanvas_Clicked(object sender, EventArgs e)
    {
        var pos = new Position(
            ((TappedEventArgs)e).GetPosition(this)!.Value.X,
            ((TappedEventArgs)e).GetPosition(this)!.Value.Y);
        pos.Round(_tileSize);

        Position = pos;
        TileTypes tileT;

        if (_grid.SpaceIsOccupied(pos) && AppState.IsRunning == false)
        {
            Navigation.PushModalAsync(new PropertiesMenu(_grid.GetTile(pos), _grid));
        }
        else if (String.IsNullOrEmpty(TileType) && AppState.IsRunning == false)
        {
            //Check for free space and then
            Navigation.PushModalAsync(new AddMenu());
        }

        try
        {
            tileT = Enum.Parse<TileTypes>(TileType!);
            TileType = null;
        }
        catch
        {
            return;
        }

        var tile = TileFactory.CreateTile(pos, _tileSize, tileT);

        var result = _grid.CanAddTile(tile);
        if (result == ETileGrid.OUT_OF_BOUNDS)
        {
            return;
        }

        if (result == ETileGrid.ELEMENT_TO_WIDE)
        {
            DisplayAlert("", "Selected Tile cannot fit", "OK");
            return;
        }

        if (result == ETileGrid.FALSE)
        {
            _grid.RemoveTile(_grid.GetTile(tile.Position));
            Layout.Invalidate();
            return;
        }

        _grid.AddTile(tile);
        Layout.Invalidate();
    }

    private void BtnRunPause_Clicked(object sender, EventArgs e)
    {
        //TODO: Check for correct pin number in all Tiles
        AppState.Change();
        if (AppState.IsRunning == true)     // if the app is running
        {
            BtnRunPause.Source = ImageSource.FromFile("stop.png");
            MainStackLayout.BackgroundColor = Color.FromArgb("#202226");
        }
        else 
        {
            BtnRunPause.Source = ImageSource.FromFile("run.png");
            MainStackLayout.BackgroundColor = Color.FromArgb("#121212");
        }
        Layout.Invalidate();
    }

    private void BtnNavBar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new FilesMenu());
    }
}