using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles.TProperties;

namespace CourseWorkUI.UI.Tiles;

public class AnalogLed : Tile, IOutput, IDBSaveable
{
    private TPropertyName _name;
    private TPropertyPin _pin;
    private TPropertyLogical _logical;
    private TPropertyState _monitoredToDB;
    private int _value = 0;

    public AnalogLed(Position pos) : base(pos, Tile.Size, Tile.Size)
    {
        Properties.Add(_name = new TPropertyName("ALED"));
        Properties.Add(_pin = new TPropertyPin($"{TileFactory.GetAvailablePin()}"));
        Properties.Add(_logical = new TPropertyLogical());
        Properties.Add(_monitoredToDB = new TPropertyState("Save to DB"));
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        var x = Position.X + Width / 2f;
        var y = Position.Y + Height / 2f;
        var r = Width / 2.4f;

        Color primary = Color.FromArgb("#bb86fc");
        canvas.FillColor = Color.FromRgba(primary.Red*_value/255, primary.Green * _value / 255, primary.Blue * _value / 255, _value);
        canvas.StrokeColor = primary;
        canvas.StrokeSize = 5f;

        canvas.FillCircle(x, y, r);
        canvas.DrawCircle(x, y, r);
        
        DrawName(canvas, dirtyRect, _name.Value);
    }

    public override int GetPin() => _pin.GetNumber();
    public void SetValue(int value) 
    {
        _value = Math.Clamp(value, 0, 255);
        if (_logical.ConditionIsTrue(_value)) 
        {
            NotificationSender.Notify($"Value of pin {GetPin()} is {_value}");
        }
    }
    
    public bool SaveToDB => _monitoredToDB.Value;
}
