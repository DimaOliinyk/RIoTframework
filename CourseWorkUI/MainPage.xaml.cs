using CourseWorkUI.Controller;
using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.Utilities;
using CourseWorkUI.Utilities.Exceptions;
using CourseWorkUI.View;

#if WINDOWS
using Windows.Storage.Provider;
#endif

namespace CourseWorkUI;

public partial class MainPage : ContentPage
{
    public static Position Position { get; set; } = new Position(0, 0);

    // Grid responsible for saving Tiles
    public static List<TileGrid> tileGrids = new List<TileGrid>();
    private static TileGrid _currentGrid = TileGrid.Create();

    // Tile Type (gets set by other menu)
    public static string? TileType { get; set; } = null;

    public MainPage()
    {
        InitializeComponent();

        // Layout to which Tile will be added to 
        Layout.Drawable = new GraphicsViewDrawable();
        tileGrids.Add(_currentGrid);
        LblProjectsName.Text = FileController.GetProjectName();

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
                _currentGrid.DrawPoints(canvas, dirtyRect, Tile.Size);

            _currentGrid.RedrawOnCanvas(canvas, dirtyRect);
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
        if (AppState.IsRunning)     // if the app is running
        {
            BtnRunPause.Source = ImageSource.FromFile("stop.png");
            MainStackLayout.BackgroundColor = ColorDictionary.TileBackground;
            BtnNavBar.IsVisible = false;
            BtnAddPage.IsVisible = false;
        }
        else
        {
            BtnRunPause.Source = ImageSource.FromFile("run.png");
            MainStackLayout.BackgroundColor = ColorDictionary.Background;
            BtnNavBar.IsVisible = true;
            BtnAddPage.IsVisible = true;
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
        try
        {
            Navigation.PushModalAsync(new FilesMenu(tileGrids));
        }
        catch (SinglePageException)
        {
            DisplayAlert("", "Cannot open this page twice", "OK");
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
        var tempPos = new Position(pos.X, pos.Y);
        pos.Round(Tile.Size);

        Position = pos;

        // Check if there is a Tile
        if (_currentGrid.SpaceIsOccupied(pos))
        {
            Tile tile = _currentGrid.GetTile(pos)!;

            if (AppState.IsRunning)     // If the app is running
            {
                tile.Clicked(tempPos);
                Layout.Invalidate();
                return;
            }

            try
            {
                Navigation.PushModalAsync(new PropertiesMenu(tile, _currentGrid));     
            }
            catch (SinglePageException) {}
        }
        else if (String.IsNullOrEmpty(TileType) && AppState.IsRunning == false)
        {
            try
            {
                Navigation.PushModalAsync(new AddMenu()); 
            }
            catch (SinglePageException) {}
        }
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
            var tile = TileFactory.CreateTile(Position, Tile.Size, tileT);

            // Checks for whether tile can be added
            var result = _currentGrid.CanAddTile(tile);
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

            _currentGrid.AddTile(tile);
        }
        if (((ModalEventArgs)e).Modal is FilesMenu) 
        {
            HandleBottomBtnCreation();
        }
        Layout.Invalidate();
    }

    /// <summary>
    /// Adds new page to project
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnAddPage_Clicked(object sender, EventArgs e)
    {
        try
        {
            var button = BottomBarButton.CreateBarButton();
            BtnFirstPage.IsVisible = (BottomBarButton.Count != 1) ? true : false;      // TODO: Access counter and check wheteher one page has to be shown

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += BtnBottomNav_Clicked;
            button.GestureRecognizers.Add(tapGestureRecognizer);

            var upSwipeGesture = new SwipeGestureRecognizer 
            { 
                Direction = SwipeDirection.Up 
            };
            upSwipeGesture.Swiped += OnSwiped;
            button.GestureRecognizers.Add(upSwipeGesture);
            
            BottomBar.Add(button);
            tileGrids.Add(TileGrid.Create());
        }
        catch (InvalidOperationException ex)
        {
            DisplayAlert("", ex.Message, "Ok");
        }
    }

    /// <summary>
    /// When any bottom nav bar button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnBottomNav_Clicked(object sender, EventArgs e)
    {
        // Get the index of grid by the pages number
        _currentGrid = tileGrids[int.Parse(
                                        ((Button)sender)
                                        .Text[4]
                                        .ToString()) - 1];
        Layout.Invalidate();
    }

    /// <summary>
    /// When page button is swiped it gets cleared
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnSwiped(object sender, SwipedEventArgs e)
    {
        if (AppState.IsRunning) return;

        var index = int.Parse(((Button)sender)
                               .Text[4]
                               .ToString()) - 1;
        tileGrids.Remove(tileGrids[index]);

        HandleBottomBtnCreation();
        BtnFirstPage.IsVisible = (BottomBarButton.Count != 1) ? true : false;
        Layout.Invalidate();
    }

    /// <summary>
    /// Creates buttons at hte bottom nav bar
    /// </summary>
    private void HandleBottomBtnCreation() 
    {
        foreach (var btn in BottomBarButton.AddedButtons)
            BottomBar.Children.Remove(btn);
        BottomBarButton.ClearAddedBtns();
        foreach (var grid in tileGrids[1..])
        {
            try
            {
                var button = BottomBarButton.CreateBarButton();
                BtnFirstPage.IsVisible = (BottomBarButton.Count != 1) ? true : false;

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += BtnBottomNav_Clicked;
                button.GestureRecognizers.Add(tapGestureRecognizer);

                var upSwipeGesture = new SwipeGestureRecognizer 
                { 
                    Direction = SwipeDirection.Up 
                };
                upSwipeGesture.Swiped += OnSwiped;
                button.GestureRecognizers.Add(upSwipeGesture);

                BottomBar.Add(button);
            }
            catch (InvalidOperationException)
            {
                DisplayAlert("", "Error while opening file occurred", "Ok");
            }
        }
        LblProjectsName.Text = FileController.GetProjectName();
        _currentGrid = tileGrids[0];
    }
}