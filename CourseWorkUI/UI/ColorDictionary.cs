namespace CourseWorkUI.UI;

/// <summary>
/// Class, collection of the colors
/// </summary>
public static class ColorDictionary
{
    public static bool DarkTheme { get; private set; } =
        (Application.Current.RequestedTheme == AppTheme.Dark);
    public static void ChangeTheme() => DarkTheme = !DarkTheme;

    // TODO: Get all values in static constructor and set colors there to save on time
    public static Color Background
    {
        get 
        {
            return (DarkTheme) ? Color.FromArgb("#121212") : Color.FromArgb("#e5e5e5");
        }
    }

    public static Color TileBackground
    {
        get
        {
            return (DarkTheme) ? Color.FromArgb("#202226") : Color.FromArgb("#ffffff");
        }
    }

    public static Color TextColor
    {
        get
        {
            return (DarkTheme) ? Color.FromArgb("#939599") : Color.FromArgb("#bb86fc");
        }
    }

    public static Color TransparentPrimary 
    {
        get 
        {
            return Color.FromArgb("#abbb86fc");
        }
    }

    public static Color Primary
    {
        get
        {
            return Color.FromArgb("#bb86fc");
        }
    }
}
