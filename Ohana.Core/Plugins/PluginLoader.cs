using System.Reflection;

namespace Ohana.Core.Plugins;

public class PluginLoader
{
    private const string SEARCH_PATTERN = "*.dll";

    public IEnumerable<IPlugin> LoadFromDirectory(string directory)
    {
        foreach (var file in Directory.EnumerateFiles(directory, SEARCH_PATTERN))
        {
            var assembly = Assembly.LoadFrom(file);

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || !typeof(IPlugin).IsAssignableFrom(type))
                    continue;

                if (Activator.CreateInstance(type) is IPlugin plugin)
                    yield return plugin;
            }
        }
    }
}