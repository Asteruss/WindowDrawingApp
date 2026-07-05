namespace WindowDrawingApp.Models;

public class Picture : WindowNode, IAttachable
{
    public double Height { get; set; }
    public double Width { get; set; }   
    public WindowFrame WindowFrame { get; set; }
    public List<ExtensionJamb> ExtensionJambs { get; } = new List<ExtensionJamb>();

    public Picture(double width, double height)
    {
        Width = width;
        Height = height;
        WindowFrame ??= new WindowFrame(Width, Height);
    }
}
