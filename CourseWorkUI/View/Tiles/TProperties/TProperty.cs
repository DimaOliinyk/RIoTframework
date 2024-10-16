namespace CourseWorkUI.UI.Tiles.TProperties;

/// <summary>
/// Abstract class representing property of a Tile
/// </summary>
public abstract class TProperty
{
    //TODO: Make property virtual and override it in other classes
    public string Value { get; set; }   // Value which TProperty holds

    protected TProperty(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates visual appearence of the property 
    /// with Maui.Controls.Entry as input and 
    /// adds as child of Maui.Controls.Grid.
    /// 
    /// Returned value allows to bind to properties 
    /// value
    /// </summary>
    /// <param name="vs"> Grid to which appearence of property will be added </param>
    /// <param name="rowCount"> Row number of passed Grid at which property will be added </param>
    /// <returns>Maui.Controls.Entry</returns>
    abstract public IView ToXaml(Grid vs, int rowCount);

    /// <summary>
    /// Checks for correctness of the value of property
    /// </summary>
    /// <returns></returns>
    public virtual bool IsCorrect() { return true; }
}
