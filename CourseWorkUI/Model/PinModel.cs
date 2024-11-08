namespace CourseWorkUI.Model;

public class PinModel
{
    public PinModel(int pinNumber, int value)
    {
        Number = pinNumber;
        Value = value;
    }

    public int Number { get; set; }
    public int Value { get; set; }

    public override string ToString()
    {
        return $"{Number}: {Value}";
    }
}
