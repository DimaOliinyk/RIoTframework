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
    private static float _tileSize;     // Size of Tile

    // TODO: Minimize code
    // Grid responsible for saving Tiles
#if WINDOWS
    private static TileGrid _grid = new TileGrid(App.WindowWidth - 50, //360
                                                 Math.Round(
                                                     App.WindowHeight /
                                                     (App.WindowWidth)) *
                                                     App.WindowWidth - 60f);
#else
    private static TileGrid _grid = new TileGrid(DeviceDisplay.Current.MainDisplayInfo.Width / 3, //360
                                                 Math.Round(
                                                     DeviceDisplay.Current.MainDisplayInfo.Height /
                                                     (DeviceDisplay.Current.MainDisplayInfo.Width)) *
                                                     DeviceDisplay.Current.MainDisplayInfo.Width / 3 - 60);   //780
#endif

    // Tile Type (gets set by other menu)
    public static string? TileType { get; set; } = null;

    public MainPage()
    {
        InitializeComponent();

        // Layout to which Tile will be added to 
        Layout.Drawable = new GraphicsViewDrawable();

        // Tile size
#if WINDOWS
        _tileSize = (App.WindowWidth - 50) / 2f;
#else
        _tileSize = (float)DeviceDisplay.Current.MainDisplayInfo.Width / 6f;
#endif
        App.Current.ModalPopping += HandleModalPopping;    // Set event handler for page popping

        // Event handler for theme changing
        Application.Current.RequestedThemeChanged += (s, a) =>
        {
            ColorDictionary.ChangeTheme();  // Change theme in color dictionary

            // Check Background color
            MainStackLayout.BackgroundColor = (AppState.IsRunning == true) ?
                ColorDictionary.TileBackground :
                ColorDictionary.Background;

            Layout.Invalidate();
        };
    }

    /// <summary>
    /// Internal class which draws all dynamic GUI
    /// </summary>
    internal class GraphicsViewDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (!AppState.IsRunning)
                _grid.DrawPoints(canvas, dirtyRect, _tileSize);

            _grid.RedrawOnCanvas(canvas, dirtyRect);
        }
    }

    /// <summary>
    /// Event handler for when 
    /// the canvas is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MCanvas_Clicked(object sender, EventArgs e)
    {
        // Get position and round it
        var pos = new Position(
            (float)((TappedEventArgs)e).GetPosition(this)!.Value.X,
            (float)((TappedEventArgs)e).GetPosition(this)!.Value.Y);
        pos.Round(_tileSize);

        Position = pos;

        // Check if there is a Tile
        if (_grid.SpaceIsOccupied(pos))
        {
            Tile tile = _grid.GetTile(pos)!;

            if (AppState.IsRunning)     // If the app is running
            {
                tile.Clicked();
                Layout.Invalidate();
                return;
            }

            Navigation.PushModalAsync(new PropertiesMenu(tile, _grid));     // Dependency injection 💉
        }
        else if (String.IsNullOrEmpty(TileType) && AppState.IsRunning == false)
        {
            Navigation.PushModalAsync(new AddMenu());
        }
    }

    /// <summary>
    /// Event handler for when the Run/Pause button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnRunPause_Clicked(object sender, EventArgs e)
    {
        //TODO: Check for correct pin number in all Tiles at runtime and then show as errors
        AppState.Change();
        if (AppState.IsRunning == true)     // if the app is running
        {
            BtnRunPause.Source = ImageSource.FromFile("stop.png");
            MainStackLayout.BackgroundColor = ColorDictionary.TileBackground;
        }
        else 
        {
            BtnRunPause.Source = ImageSource.FromFile("run.png");
            MainStackLayout.BackgroundColor = ColorDictionary.Background;
        }
        Layout.Invalidate();
    }

    /// <summary>
    /// Event handler for when Navigation Bar button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnNavBar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new FilesMenu());
    }

    /// <summary>
    /// Event handler for when any page 
    /// (PropertiesMenu, AddMenu, FilesMenu)
    /// gets closed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleModalPopping(object sender, EventArgs e) 
    {
        // if the closed page was AddMenu
        if (((ModalEventArgs)e).Modal is AddMenu)
        {
            // Saves passed TileType and tries to parse it
            TileTypes tileT;    
            try
            {
                tileT = Enum.Parse<TileTypes>(TileType!);
                TileType = null;
            }
            catch
            {
                return;
            }

            // If parsing successful - creates Tile
            var tile = TileFactory.CreateTile(Position, _tileSize, tileT);

            // Checks for whether tile can be added
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
                Layout.Invalidate();
                return;
            }

            _grid.AddTile(tile);
        }
        Layout.Invalidate();
    }
}