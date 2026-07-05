namespace WindowDrawingApp.Models.SpaceContents;

public class ImpostContent : Beam
{
    public ISpaceContent Content { get; set; }
    public ImpostType ImpostType { get; set; }
    public ImpostContent(ImpostType type = ImpostType.Physical) : base(BaseSizes.BeamWidth, 0)
    {
        ImpostType = type;
        WidthChanged += Resize;
        Content = new EmptySpaceContent();
    }
    public void Resize()
    {
        if (Content != null && Length > 0)
        {
            Content.Resize(Length, Width);
        }
    }
}

public enum ImpostType
{
    Physical, Virtual
}