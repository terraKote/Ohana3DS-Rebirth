using System.Text;
using Ohana.Core.Assets;
using Ohana.Core.FileFormats;

namespace Ohana.Plugins.CGFX;

public struct FileHeader
{
    public string Magic { get; }
    public ushort Endianness { get; }
    public ushort Length { get; }
    public uint Revision { get; }
    public uint FileLength { get; }
    public uint EntryCount { get; }

    public FileHeader(string magic, ushort endianness, ushort length, uint revision, uint fileLength, uint entryCount)
    {
        Magic = magic;
        Endianness = endianness;
        Length = length;
        Revision = revision;
        FileLength = fileLength;
        EntryCount = entryCount;
    }
}

public class CgfxFileImporter : IFileImporter
{
    private const string MAGIC = "CGFX";

    public FileFormatDescriptor FormatDescriptor => new("CGFX", [".fs", ".bcres"]);

    public bool CanImport(Stream stream)
    {
        var bytes = new byte[MAGIC.Length];

        for (int i = 0; i < bytes.Length; i++)
        {
            var b = stream.ReadByte();

            if (b == 0x00)
                return false;

            bytes[i] = (byte)b;
        }

        var magic = Encoding.ASCII.GetString(bytes);
        return string.Equals(magic, MAGIC, StringComparison.Ordinal);
    }

    public IAsset Import(Stream stream)
    {
        var buffer = new byte[4];
        stream.ReadExactly(buffer);
        var magicString = Encoding.ASCII.GetString(buffer);
        
        stream.ReadExactly(buffer, 0, sizeof(ushort));
        var endianness = BitConverter.ToUInt16(buffer, 0);
        
        stream.ReadExactly(buffer, 0, sizeof(ushort));
        var length = BitConverter.ToUInt16(buffer, 0);
        
        stream.ReadExactly(buffer, 0, sizeof(uint));
        var revision = BitConverter.ToUInt32(buffer, 0);
        
        stream.ReadExactly(buffer, 0, sizeof(uint));
        var fileLength = BitConverter.ToUInt32(buffer, 0);
        
        stream.ReadExactly(buffer, 0, sizeof(uint));
        var entryCount = BitConverter.ToUInt32(buffer, 0);
        
        var fileHeader = new FileHeader(magicString, endianness, length, revision, fileLength, entryCount);

        return null;
    }
}