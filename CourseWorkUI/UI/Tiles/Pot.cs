using CourseWorkUI.Model;
using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles.TProperties;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.View.Tiles;

public class Pot : Tile, IInput
{
    private TPropertyName _name;
    private TPropertyPin _pin;
    private TPropertyValue<float> _min;    // Min value property
    private TPropertyValue<float> _max;    // Max value property
    private TPropertyState _isSimple;
    private double _value = 127;

    private float _sliderSize;
    private float _meterWidth;

    public Pot(Position pos) : base(pos, 2*Tile.Size, Tile.Size)
    {
        _name = new TPropertyName("Pot");
        _pin = new TPropertyPin($"{TileFactory.GetAvailablePin()}");
        _min = new TPropertyValue<float>("0", "Min");
        _max = new TPropertyValue<float>("255", "Max");
        _isSimple = new TPropertyState("Simplify");

        Properties.Add(_name);
        Properties.Add(_pin);
        Properties.Add(_min);
        Properties.Add(_max);
        Properties.Add(_isSimple);

        _sliderSize = Height / 3.5f;
        _meterWidth = Width - 40 - _sliderSize;
}

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();

        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = Height / 9f;
        canvas.FontColor = ColorDictionary.TextColor;

        canvas.DrawString(
            $"{_value:0.00}",
            Position.X + Width / 2f,
            Position.Y + Height / 3.5f,
            HorizontalAlignment.Center);

        float xStart = Position.X + 20 + _sliderSize/2f;
        float meterHeight = Height * 0.2f;
        float yStart = Position.Y + 2.5f*meterHeight;

        float valuePos = xStart + _meterWidth * ((float)_value - _min.GetNumber())
                            / (_max.GetNumber() - _min.GetNumber()) - _sliderSize / 2f;

        if (_isSimple.Value)
        {
            canvas.FillColor = ColorDictionary.TransparentPrimary;
            canvas.FillRectangle(
                xStart,
                yStart,
                _meterWidth,
                meterHeight);
        }
        else 
        {
            var gradient = new LinearGradientPaint
            {
                StartColor = ColorDictionary.TransparentPrimary,
                EndColor = ColorDictionary.TileBackground,  
                StartPoint = (ColorDictionary.DarkTheme) ? new Point(0, 0) : new Point(1, 0),
                EndPoint = (ColorDictionary.DarkTheme) ? new Point(1, 0) : new Point(0, 0),
            };

            canvas.SetFillPaint(gradient, 
                new RectF(Position.X, Position.Y, valuePos, Position.Y+meterHeight));
        }
        // Progress bar
        canvas.FillRectangle(
                xStart,
                yStart,
                valuePos,
                meterHeight);

        canvas.RestoreState();

        canvas.FillColor = (_isSimple.Value) ? Colors.Grey : ColorDictionary.Primary;
        canvas.FillRoundedRectangle(
            valuePos,
            yStart + meterHeight / 2f - _sliderSize / 2f,
            _sliderSize,
            _sliderSize,
            5f);

        DrawName(canvas, dirtyRect, _name.Value);
    }

    public override void Clicked(Position pos)
    {
        _value = (pos.X - Position.X) / Width * (_max.GetNumber() - _min.GetNumber());
    }

    public override int GetPin() => _pin.GetNumber();

    public int GetInputValue() 
    {
        try
        {
            return (int)_value;
        }
        catch (Exception )
        {
            return 0;
        }
    }
}
