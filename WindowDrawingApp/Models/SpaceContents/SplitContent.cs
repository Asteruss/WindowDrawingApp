namespace WindowDrawingApp.Models.SpaceContents;

public class SplitContent : ISpaceContent
{
    private double _contentHeight;
    private double _contentWidth;
    public ImpostContent Impost { get; init; }
    public LightSpace Space1 { get; }
    public LightSpace Space2 { get; }
    private double _offset;
    public double Offset
    {
        get => _offset; set
        {
            _offset = value;
            _checkImpostOffset();
            _recalculateChildren();
        }
    }

    private readonly SplitOrientation _orientation;
    public void Resize(double width, double height)
    {
        _contentHeight = height;
        _contentWidth = width;

        Impost.Length = (_orientation == SplitOrientation.Vertical) ? height : width;
        Impost.Resize();

        _checkImpostOffset();
        _recalculateChildren();
    }

    public SplitContent(ImpostContent impost, double offset, SplitOrientation orientation = SplitOrientation.Vertical)
    {
        _orientation = orientation;

        _offset = offset;

        Impost = impost;
        Impost.WidthChanged += _recalculateChildren;

        Space1 = new LightSpace();
        Space2 = new LightSpace();
    }

    private void _recalculateChildren()
    {
        if (_orientation == SplitOrientation.Vertical)
        {
            double space1Width = _offset;
            double space2Width = _contentWidth - _offset - Impost.Width;

            Space1.Resize(space1Width, _contentHeight);
            Space2.Resize(space2Width, _contentHeight);
        }
        else
        {
            double space1Height = _offset;
            double space2Height = _contentHeight - _offset - Impost.Width;

            Space1.Resize(_contentWidth, space1Height);
            Space2.Resize(_contentWidth, space2Height);
        }
    }

    private void _checkImpostOffset()
    {
        double maxOffset = (_orientation == SplitOrientation.Vertical)
            ? _contentWidth - Impost.Width
            : _contentHeight - Impost.Width;

        if (_offset < 0) _offset = 0;
        if (_offset > maxOffset) _offset = maxOffset;
    }
}

public enum SplitOrientation
{
    Vertical,
    Horizontal
}