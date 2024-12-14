using CourseWorkUI.Controller;
using System.Diagnostics;

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
    private static bool _isRunning = false;
    public static bool IsRunning
    {
        get => _isRunning;
        private set
        {
            Debug.WriteLine($"AppState {_isRunning}  -> {value}");
            _isRunning = value;
        }
    }

    /// <summary>
    /// Changes the mode to the opposite (Running/Editing)
    /// </summary>
    public static void Change() => IsRunning = !IsRunning;
    public static void TurnOff() => IsRunning = false;
}


public static class IDLEState 
{
    private static bool _isIdle = false;
    public static bool IsIdle 
    { 
        get => _isIdle; 
        private set 
        { 
            Debug.WriteLine($"IDLEState {_isIdle} -> {value}");
            _isIdle = value;
        } 
    }

    public static void Change() => IsIdle = !IsIdle;
    public static void TurnOff() => IsIdle = false;
}