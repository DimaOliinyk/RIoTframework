using CourseWorkUI.Utilities.Exceptions;
using Microsoft.Maui.Controls.Shapes;
namespace CourseWorkUI.UI.Menues;

/// <summary>
/// Menu responsible for asking the user 
/// types of Tiles
/// </summary>
public partial class AddMenu : ContentPage
{
    private static bool _pageIsOpen = false;

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="SinglePageException"></exception>
	public AddMenu()
	{
        if (_pageIsOpen)
            throw new SinglePageException();
        _pageIsOpen = true;

		InitializeComponent();

        // Gets all Enum values and adds to CollectionView (VwCollectionView)
        var tileTypesArr = Enum.GetValues(typeof(TileTypes));	
		var strTileTypes = new List<string>();
        var itemTable = new Dictionary<Border, string>();       // Saves items hash to decide which one was picked
        
		foreach (var type in tileTypesArr) 
		{
            strTileTypes.Add(type.ToString()!);	
		}


        foreach (var item in strTileTypes)
        {
            // Add to item of list and add event to recognize clicking
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>    // Clicked event
            {
                foreach (var item in VSItemList)    // Every item in Vertical Stack
                {
                    if (s.GetHashCode() == item.GetHashCode())  // Compare hash codes
                    {
                        itemTable.TryGetValue((Border)s, out string type);
                        AddTile(type!);
                        break;
                    }
                }
            };

            // Create item and event to it and it to Vertical Stack and hash table
            var border = CreateItem(item, $"icon_{item.ToLower()}.png");
            border.GestureRecognizers.Add(tapGestureRecognizer);
            VSItemList.Add(border);
            itemTable.Add(border, item);
        }
    }

    /// <summary>
    /// Creates Item to be displayed
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="imagePath"></param>
    /// <returns></returns>
    private Border CreateItem(string itemName, string imagePath) 
    {
        var grid = new Grid
        {
            RowDefinitions = 
            {
                new RowDefinition(), 
                new RowDefinition() 
            },

        };

        grid.Add(new Label
        {
            Text = itemName,
            TextColor = ColorDictionary.TextColor,
            BackgroundColor = ColorDictionary.Background,
            HorizontalOptions = LayoutOptions.Start,
        }, 0, 0);

        grid.Add(new Image
        {
            Source = ImageSource.FromFile(imagePath),
            MaximumHeightRequest = 120,
        }, 0, 1);
        
        return new Border
        {
            Stroke = new LinearGradientBrush
            {
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection
                        {
                            new GradientStop { Color = ColorDictionary.Background, Offset = 0.1f },
                            new GradientStop { Color = ColorDictionary.Primary, Offset = 1.0f }
                        },
            },
            Background = ColorDictionary.Background,
            StrokeThickness = 2,
            Padding = new Thickness(32, 16),
            Margin = new Thickness(5, 0, 5, 5),
            VerticalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(40, 0, 0, 40)
            },
            Content = grid,
        };
    }

    /// <summary>
    /// Event handler for when the item 
    /// in the collection view is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddTile(string tileType)
    {
        // Sets a static field in the main page responcible for tile type
        MainPage.TileType = tileType;
        Navigation.PopModalAsync();
    }

    /// <summary>
    /// Event handler for when the Cnacel button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
        // Sets a static field in the main page responsible for tile type
        MainPage.TileType = null;
        Navigation.PopModalAsync();
    }

    protected override void OnDisappearing() => _pageIsOpen = false;
}