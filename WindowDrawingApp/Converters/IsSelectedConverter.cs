using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WindowDrawingApp.Models;

namespace WindowDrawingApp.Converters;

public class IsSelectedConverter : IMultiValueConverter
{
    // values[0] - текущий элемент
    // values[1] - SelectedNode из MainViewModel
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] == null || values[1] == null) return false;
        if (values.Length == 2 && values[0] is WindowNode currentNode && values[1] is WindowNode selectedNode)
        {
            return ReferenceEquals(currentNode, selectedNode);
        }

        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}