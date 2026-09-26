namespace Ohana.Core.FileFormats;

public sealed record FileFormatDescriptor(string Name, IReadOnlyList<string> Extensions);

public interface IFileImporter
{
    FileFormatDescriptor FormatDescriptor { get; }
}