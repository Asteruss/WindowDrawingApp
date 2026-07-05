namespace WindowDrawingApp.Models;

// брус
// длина фиксированна к габаритам
// ширина изменяема
public class Beam : WindowNode
{
    private double _width;
    public double Width
    {
        get => _width;
        set
        {
            if (_width != value)
            {
                _width = value;
                WidthChanged?.Invoke(); 
            }
        }
    }
    public double Length { get; set; }

    public Beam(double width, double length)
    {
        _width = width;
        Length = length;
    }
    public event Action WidthChanged;
}