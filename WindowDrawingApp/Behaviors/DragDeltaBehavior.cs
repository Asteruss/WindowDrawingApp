using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WindowDrawingApp.Behaviors;

public static class DragDeltaBehavior
{
    public static readonly DependencyProperty DragCommandProperty =
        DependencyProperty.RegisterAttached("DragCommand", typeof(ICommand), typeof(DragDeltaBehavior), new PropertyMetadata(null, OnDragCommandChanged));

    public static ICommand GetDragCommand(DependencyObject obj) => (ICommand)obj.GetValue(DragCommandProperty);
    public static void SetDragCommand(DependencyObject obj, ICommand value) => obj.SetValue(DragCommandProperty, value);

    private static void OnDragCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Thumb thumb)
        {
            thumb.DragDelta -= Thumb_DragDelta;
            if (e.NewValue is ICommand)
                thumb.DragDelta += Thumb_DragDelta;
        }
    }

    private static void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (sender is Thumb thumb && GetDragCommand(thumb) is ICommand command)
        {
            if (command.CanExecute(null))
            {
                double change = 0;

                bool isHorizontal = thumb.HorizontalAlignment != HorizontalAlignment.Stretch &&
                                    thumb.HorizontalAlignment != HorizontalAlignment.Center;

                if (isHorizontal)
                {
                    change = e.HorizontalChange;
                    if (thumb.HorizontalAlignment == HorizontalAlignment.Left) change = -change;
                }
                else
                {
                    change = e.VerticalChange;
                    if (thumb.VerticalAlignment == VerticalAlignment.Top) change = -change;
                }

                command.Execute((thumb.DataContext, change));
            }
        }
    }
}