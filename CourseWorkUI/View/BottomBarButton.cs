using System.Runtime.CompilerServices;

namespace CourseWorkUI.View;

/// <summary>
/// Creates buttons for bottom bar
/// </summary>
public static class BottomBarButton
{
    // Counts the number of all botoom bar buttons
    public static int Count { get; private set; } = 1;
    public static List<Button> AddedButtons { get; private set; } = new List<Button>();

    /// <summary>
    /// Creates instances of buttons
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Button CreateBarButton() 
    {
        if (Count == 3) 
        {
            throw new InvalidOperationException("Cannot create more than three pages");
        }

        var newBtn = new Button
        {
            Text = $"Page{++Count}",
            Style = (Style)Application.Current.Resources["Bar"],
        };

        AddedButtons.Add(newBtn);

        return newBtn;
    }

    internal static void ClearAddedBtns()
    {
        AddedButtons.Clear();
        ResetCounter();
    }

    public static void ResetCounter() => Count = 1;
}
