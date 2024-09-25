namespace CourseWorkUI;

/// <summary>
/// Represents the current state of the App 
/// </summary>
public static class AppState
{
    /// <summary>
    /// Allows to check whether the app 
    /// is in the running or editing mode 
    /// </summary>
    public static bool IsRunning { get; private set; } = false;

    /// <summary>
    /// Changes the mode to the opposite (Running/Editing)
    /// </summary>
    public static void Change() => IsRunning = !IsRunning;
}
