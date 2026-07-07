namespace WindowDrawingApp.Models;

public class WindowFrame : WindowNode
{
    private double _windowWidth = 0;
    private double _windowHeight = 0;
    public FrameType _frameType = FrameType.Vertical;
    public Beam Left { get; }
    public Beam Right { get; }
    public Beam Top { get; }
    public Beam Bottom { get; }
    public FrameType FrameType
    {
        get => _frameType;
        set { SetProperty(ref _frameType, value); Recalculate(); }
    }

    public LightSpace RootSpace { get; private set; }

    public WindowFrame(double windowWidth, double windowHeight, FrameType frameType = FrameType.Vertical)
    {
        _windowHeight = windowHeight;
        _windowWidth = windowWidth;
        _frameType = frameType;
        RootSpace = new LightSpace();


        Left = new Beam(BaseSizes.BeamWidth, _windowHeight) { DisplayName="Левый брус рамы", Parent=this};
        Right = new Beam(BaseSizes.BeamWidth, _windowHeight) { DisplayName = "Правый брус рамы", Parent = this };
        Top = new Beam(BaseSizes.BeamWidth, _windowWidth - Left.Width - Right.Width) { DisplayName = "Верхний брус рамы", Parent = this };
        Bottom = new Beam(BaseSizes.BeamWidth, _windowWidth - Left.Width - Right.Width) { DisplayName = "Нижний брус рамы", Parent = this };

        Left.WidthChanged += Recalculate;
        Right.WidthChanged += Recalculate;
        Top.WidthChanged += Recalculate;
        Bottom.WidthChanged += Recalculate;

        Recalculate();
    }

    private void Recalculate()
    {
        switch (_frameType)
        {
            case FrameType.Vertical:
                RecalculateVertically();
                break;
            case FrameType.Horizontal:
                RecalculateHorizontal();
                break;
            default:
                throw new NotImplementedException();
        }

        double spaceWidth = _windowWidth - Left.Width - Right.Width;
        double spaceHeight = _windowHeight - Top.Width - Bottom.Width;
        RootSpace.Resize(spaceWidth, spaceHeight);
    }
    
    // длина левого и правого бруса в размер окна
    private void RecalculateVertically()
    {
        Left.Length = _windowHeight;
        Right.Length = _windowHeight;

        double horizontalLength = _windowWidth - Left.Width - Right.Width;
        Top.Length = horizontalLength;
        Bottom.Length = horizontalLength;
    }

    // длина верхнего и нижнего бруса в размер окна
    private void RecalculateHorizontal()
    {
        Top.Length = _windowWidth;
        Bottom.Length = _windowWidth;

        double verticalLength = _windowHeight - Top.Width - Bottom.Width;
        Left.Length = verticalLength;
        Right.Length = verticalLength;
    }
}

public enum FrameType
{
    Horizontal, Vertical
}