using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ohana.Core.FileFormats;
using Ohana.Core.Plugins;
using OhanaNext.Events;
using OhanaNext.Services;

namespace OhanaNext.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IApplicationService _applicationService;
        private readonly IFilePickerService _filePickerService;
        private readonly IPluginRegistry _pluginRegistry;
        private readonly IEventService _eventService;

        public MainWindowViewModel(IApplicationService applicationService, IFilePickerService filePickerService,
            IPluginRegistry pluginRegistry, IEventService eventService)
        {
            _applicationService = applicationService;
            _filePickerService = filePickerService;
            _pluginRegistry = pluginRegistry;
            _eventService = eventService;
        }

        [RelayCommand]
        private void Open()
        {
            var importers = _pluginRegistry.GetAll<IFileImporter>();
            var descriptors = importers.Select(x => x.FormatDescriptor).ToArray();
            var path = _filePickerService.PickFile(descriptors);

            if (string.IsNullOrEmpty(path))
                return;

            using var stream = File.OpenRead(path);
            IFileImporter selectedImporter = null;

            foreach (var importer in importers)
            {
                stream.Position = 0;

                if (!importer.CanImport(stream))
                    continue;

                selectedImporter = importer;
                break;
            }

            if (selectedImporter == null)
            {
                _applicationService.ShowMessageBox($"Couldn't import file {path}");
                return;
            }

            try
            {
                stream.Position = 0;
                var asset = selectedImporter.Import(stream);
                _eventService.Trigger(new AssetLoadedEvent(asset));
            }
            catch (Exception e)
            {
                _applicationService.ShowMessageBox(e.Message);
                throw;
            }
        }

        [RelayCommand]
        private void Exit()
        {
            _applicationService.Exit();
        }
    }
}