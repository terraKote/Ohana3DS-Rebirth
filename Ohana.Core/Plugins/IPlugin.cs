namespace Ohana.Core.Plugins;

public interface IPlugin
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
}