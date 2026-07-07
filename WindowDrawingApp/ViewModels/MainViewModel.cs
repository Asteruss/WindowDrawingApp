using System.Collections.ObjectModel;
using System.Windows;
using WindowDrawingApp.Models;

namespace WindowDrawingApp.ViewModels;

public class MainViewModel : NotifyPropertyChanged
{
    private Picture _currentPicture;
    public Picture CurrentPicture { get => _currentPicture; set=>SetProperty(ref _currentPicture, value);  }

    private WindowNode _selectedNode;
    public WindowNode SelectedNode { get => _selectedNode; set => SetProperty(ref _selectedNode, value); }
    public ObservableCollection<WindowNode> Nodes { get; set; }

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

            CurrentPicture = new Picture(600, 800);
        });
    }
    private RelayCommand<(object Target, double Delta)> _dragBeamCommand;
    public RelayCommand<(object Target, double Delta)> DragBeamCommand { get => _dragBeamCommand ??= new(ExecuteDragBeam); }

    private void ExecuteDragBeam((object Target, double Delta) args)
    {
        if (args.Target is Beam beam)
            if (args.Delta != 0)
                beam.Width = Math.Max(10, beam.Width + args.Delta);
    }

    private RelayCommand<object> _selectNodeCommand;
    public RelayCommand<object> SelectNodeCommand { get => _selectNodeCommand ??= new(node => SelectedNode = node as WindowNode); }
}
