using Ohana.Core.Plugins;

namespace OhanaNext;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new App();
        app.InitializeComponent();

        var pluginLoader = new PluginLoader();
        var plugins = pluginLoader.LoadFromDirectory("./").ToArray();

        var window = new MainWindow();
        app.Run(window);
    }
}