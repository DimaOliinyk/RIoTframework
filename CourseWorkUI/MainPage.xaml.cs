using CourseWorkUI.Controller;
using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Menues.IDEL;
using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;
using CourseWorkUI.Utilities.Exceptions;
using CourseWorkUI.View;

namespace CourseWorkUI;

public partial class MainPage : ContentPage
{
    public static Position Position { get; set; } = new Position(0, 0);

    // Grid responsible for saving Tiles
    public static List<TileGrid> tileGrids = new List<TileGrid>();
    private static TileGrid _currentGrid = TileGrid.Create();

    // Tile Type (gets set by other menu)
    public static string? TileType { get; set; } = null;

    // Event for updating screen
    public void UpdateScreenFunc() => Layout.Invalidate();

    public MainPage()
    {
        InitializeComponent();

        CircuitController.UpdateScreen += UpdateScreenFunc;
        // Layout to which Tile will be added to 
        Layout.Drawable = new GraphicsViewDrawable();
        tileGrids.Add(_currentGrid);
        LblProjectsName.Text = FileController.GetProjectName();

        App.Current!.ModalPopping += HandleModalPopping!;    // Set event handler for page popping

        // Event handler for theme changing
        Application.Current.RequestedThemeChanged += (s, a) =>
        {
            ColorDictionary.ChangeTheme();  // Change theme in color dictionary

            // Check Background color
            MainStackLayout.BackgroundColor = (AppState.IsRunning) ?
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
    private async void BtnRunPause_Clicked(object sender, EventArgs e)
    {
        if (!WIFIProperties.TransferDataAndCheck()) 
        {
            await DisplayAlert("","Cannot connect to device, wrong values in settings found", "OK");
            return;
        }

        AppState.Change();

        if (AppState.IsRunning)
        {
            try
            {
                await CircuitController.PrepareData(tileGrids);
            }
            catch (Exception)
            {
                await DisplayAlert("", "Connection Error", "OK");
                IDLEState.TurnOff();
            }
        }
        else 
        {
            IDLEState.TurnOff();
        }
        ChangeVisualRunningState();

        try
        {
            await CircuitController.StartDataChecking();
        }
        catch (HttpRequestException)
        {
            await DisplayAlert("", "Client not responding", "OK");
            AppState.TurnOff();
            IDLEState.TurnOff();
            ChangeVisualRunningState();
        }
        catch (UriFormatException)
        {
            await DisplayAlert("", "Invalid IP address specified", "OK");
            AppState.TurnOff();
            IDLEState.TurnOff();
            ChangeVisualRunningState();
        }
        catch (Exception ex)
        {
            await DisplayAlert("", $"Something went wrong. More info:\n{ex.Message}", "OK");
            AppState.TurnOff();
            IDLEState.TurnOff();
            ChangeVisualRunningState();
        }
    }

    private void ChangeVisualRunningState() 
    {
        BtnRunPause.Source = (AppState.IsRunning)
                                      ? ImageSource.FromFile("stop.png")
                                      : ImageSource.FromFile("run.png");
        MainStackLayout.BackgroundColor = (AppState.IsRunning)
                                                    ? ColorDictionary.TileBackground
                                                    : ColorDictionary.Background;
        BtnNavBar.IsVisible = !AppState.IsRunning;
        BtnAddPage.IsVisible = !AppState.IsRunning;
        Btnsettings.IsVisible = !AppState.IsRunning;
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
    /// Opens Settings Page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Btnsettings_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushModalAsync(new SettingsMenu());
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
        var tempPos = (Position)pos.Clone();
        pos.Round(Tile.Size);

        Position = pos;

        // Check if there is a Tile
        if (_currentGrid.SpaceIsOccupied(pos))
        {
            Tile tile = _currentGrid.GetTile(pos)!;

            if (AppState.IsRunning)     // If the app is running
            {
                tile.Clicked(tempPos);
                if (tile is IInput) 
                {
                    int pin = tile.GetPin();
                    int val = ((IInput)tile).GetInputValue();
                    try 
                    {
                        CircuitController.SendData(pin, val);
                    }
                    catch(Exception)
                    {
                        DisplayAlert("", "Cannot send pin data", "OK");
                    }
                }
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
    /// (eg. PropertiesMenu, AddMenu, FilesMenu)
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
            var tile = TileFactory.CreateTile(Position, tileT);

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
            BtnFirstPage.IsVisible = (BottomBarButton.Count != 1) ? true : false;

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer!.Tapped += BtnBottomNav_Clicked!;
            button.GestureRecognizers.Add(tapGestureRecognizer);

            var upSwipeGesture = new SwipeGestureRecognizer 
            { 
                Direction = SwipeDirection.Up 
            };
            upSwipeGesture!.Swiped += OnSwiped!;
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
                                        ((Microsoft.Maui.Controls.Button)sender)
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

        var index = int.Parse(((Microsoft.Maui.Controls.Button)sender)
                               .Text[4]
                               .ToString()) - 1;
        tileGrids[index].Clear();
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
                tapGestureRecognizer.Tapped += BtnBottomNav_Clicked!;
                button.GestureRecognizers.Add(tapGestureRecognizer);

                var upSwipeGesture = new SwipeGestureRecognizer 
                { 
                    Direction = SwipeDirection.Up 
                };
                upSwipeGesture!.Swiped += OnSwiped!;
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

    public static void IDLEData() 
    {
        CircuitController.StartIDLEDataSending(IDLEPage.TimePinVal);
    }

    public static void ReadData()
    {
        CircuitController.StartDataChecking();
    }
}