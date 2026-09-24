namespace Ohana.Core.Plugins;

public sealed record PluginDescriptor(string Name, string Description, string[] Authors, Version Version);

public interface IPlugin
{
    PluginDescriptor Descriptor { get; }

    void Register(IPluginRegistry registry);
}