using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OhanaNext.Services;

namespace OhanaNext.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IApplicationService _applicationService;

    public MainWindowViewModel(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [RelayCommand]
    private void Open()
    {
        
    }

    [RelayCommand]
    private void Exit()
    {
        _applicationService.Exit();
    }
}