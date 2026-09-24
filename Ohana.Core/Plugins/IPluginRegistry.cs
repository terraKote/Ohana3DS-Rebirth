namespace Ohana.Core.Plugins;

public interface IPluginRegistry
{
    void Register<TCapability>(TCapability capability) where TCapability : class;
}

public class PluginRegistry : IPluginRegistry
{
    private readonly HashSet<object> _capabilities = new HashSet<object>();

    public void Register<TCapability>(TCapability capability) where TCapability : class
    {
        _capabilities.Add(capability);
    }
}