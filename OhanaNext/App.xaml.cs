using System.Windows;
using Ohana.Core.Plugins;
using OhanaNext.Services;
using OhanaNext.ViewModels;
using OhanaNext.Views;

namespace OhanaNext;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var pluginLoader = new PluginLoader();
        var pluginRegistry = new PluginRegistry();
        var plugins = pluginLoader.LoadFromDirectory("./Plugins");

        foreach (var plugin in plugins)
        {
            plugin.Register(pluginRegistry);
        }

        var applicationService = new ApplicationService();
        var filePickerService = new FIlePickerService();
        
        var mainWindowViewModel = new MainWindowViewModel(applicationService, filePickerService, pluginRegistry);
        var window = new MainWindow(mainWindowViewModel);
        window.Show();
    }
}