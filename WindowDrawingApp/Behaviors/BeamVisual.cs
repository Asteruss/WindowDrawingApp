using System.Windows;

namespace WindowDrawingApp.Behaviors;

public class BeamVisual : DependencyObject
{
    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.RegisterAttached("Orientation", typeof(BeamOrientation), typeof(BeamVisual));
    public static BeamOrientation GetOrientation(DependencyObject obj) => (BeamOrientation)obj.GetValue(OrientationProperty);
    public static void SetOrientation(DependencyObject obj, BeamOrientation value) => obj.SetValue(OrientationProperty, value);

    public static readonly DependencyProperty ThumbSideProperty =
        DependencyProperty.RegisterAttached("ThumbSide", typeof(ThumbSide), typeof(BeamVisual));
    public static ThumbSide GetThumbSide(DependencyObject obj) => (ThumbSide)obj.GetValue(ThumbSideProperty);
    public static void SetThumbSide(DependencyObject obj, ThumbSide value) => obj.SetValue(ThumbSideProperty, value);
}

public enum BeamOrientation { Horizontal, Vertical }
public enum ThumbSide { Start, End }