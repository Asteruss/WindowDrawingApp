namespace WindowDrawingApp.Models;

public class WindowNode
{
    public WindowNode Parent { get; set; }
    public virtual string DisplayName { get; protected set; } = "Неизвестный элемент";
}
