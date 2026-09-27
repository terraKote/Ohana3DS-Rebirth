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