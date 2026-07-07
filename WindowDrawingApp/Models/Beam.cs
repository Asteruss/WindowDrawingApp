using WindowDrawingApp.Behaviors;

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
                if (_width <= 5)
                    _width = 5;

                WidthChanged?.Invoke();
                OnPropertyChanged();
            }
        }
    }

    private double _length;
    public double Length
    {
        get => _length;
        set { if (_length != value) { _length = value; OnPropertyChanged(); } }
    }


    public Beam(double width, double length)
    {
        _width = width;
        _length = length;
    }
    public event Action WidthChanged;
}