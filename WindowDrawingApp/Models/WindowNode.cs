using WindowDrawingApp.ViewModels;

namespace WindowDrawingApp.Models;

public class WindowNode : NotifyPropertyChanged
{
    public WindowNode Parent { get; set; }
    public virtual string DisplayName { get; set; } = "Неизвестный элемент";
}
