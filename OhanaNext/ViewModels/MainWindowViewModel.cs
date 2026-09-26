using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ohana.Core.FileFormats;
using Ohana.Core.Plugins;
using OhanaNext.Services;

namespace OhanaNext.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IApplicationService _applicationService;
    private readonly IFilePickerService _filePickerService;
    private readonly IPluginRegistry _pluginRegistry;

    public MainWindowViewModel(IApplicationService applicationService, IFilePickerService filePickerService,
        IPluginRegistry pluginRegistry)
    {
        _applicationService = applicationService;
        _filePickerService = filePickerService;
        _pluginRegistry = pluginRegistry;
    }

    [RelayCommand]
    private void Open()
    {
        var importers = _pluginRegistry.GetAll<IFileImporter>();
        var descriptors = importers.Select(x => x.FormatDescriptor).ToArray();
        var result = _filePickerService.PickFile(descriptors);
        
        if(string.IsNullOrEmpty(result))
            return;
    }

    [RelayCommand]
    private void Exit()
    {
        _applicationService.Exit();
    }
}