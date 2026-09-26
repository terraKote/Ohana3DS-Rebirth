using Ohana.Core.FileFormats;

namespace Ohana.Plugins.CGFX;

public class CgfxFileImporter : IFileImporter
{
    public FileFormatDescriptor FormatDescriptor => new("CGFX", [".fs"]);
}