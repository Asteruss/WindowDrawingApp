namespace WindowDrawingApp.Models;

public class ExtensionJamb : Beam, IAttachable
{
    public IAttachable AttachedTo { get; set; }
    public AttachSide Side { get; set; }
    public ExtensionJamb(double width, double length, IAttachable attachedTo, AttachSide side) : base(width, length)
    {
        AttachedTo = attachedTo;
        Side = side;
    }

}

public enum AttachSide
{
    Left, Top, Right, Bottom    
}
