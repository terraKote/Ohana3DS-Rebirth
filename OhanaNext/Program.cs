using Ohana.Core.Plugins;
using OhanaNext.Views;

namespace OhanaNext;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new App();
        app.InitializeComponent();

        var pluginLoader = new PluginLoader();
        var pluginRegistry = new PluginRegistry();
        var plugins = pluginLoader.LoadFromDirectory("./Plugins");

        foreach (var plugin in plugins)
        {
            plugin.Register(pluginRegistry);
        }

        var window = new MainWindow();
        app.Run(window);
    }
}