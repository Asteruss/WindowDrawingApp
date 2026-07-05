using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowDrawingApp.Models;

namespace WindowDrawingApp.ViewModels;

public class MainViewModel : NotifyPropertyChanged
{
    private Picture _currentPicture;
    public Picture CurrentPicture { get => _currentPicture; set=>SetProperty(ref _currentPicture, value);  }

    private WindowNode _selectedNode;
    public WindowNode SelectedNode { get => _selectedNode; set => SetProperty(ref _selectedNode, value); }
    private RelayCommand _createPicture;
    public RelayCommand CreatePicture
    {
        get => _createPicture ??= new(obj =>
        {
            if (CurrentPicture != null)
            {
                var res = MessageBox.Show("Вы уверены? Текущий чертеж будет потерян.", "Предупреждение", MessageBoxButton.YesNo);
                if (res != MessageBoxResult.Yes) return;
            }

            CurrentPicture = new Picture(800, 1000);
        });
    }
}
