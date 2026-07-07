using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WindowDrawingApp.Models;

namespace WindowDrawingApp.Converters;

public class FrameTypeToVisibilityConverter : IValueConverter
{
    // Vertical или Horizontal
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FrameType currentType && parameter is string targetMode)
        {
            return currentType.ToString() == targetMode ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
