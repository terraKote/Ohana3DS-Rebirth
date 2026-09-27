using System.Text;
using Ohana.Core.Assets;
using Ohana.Core.FileFormats;

namespace Ohana.Plugins.CGFX;

public class CgfxFileImporter : IFileImporter
{
    private const uint BUFFER_SIZE = 4;
    private const string FILE_HEADER_MAGIC = "CGFX";
    private const string DATA_HEADER_MAGIC = "DATA";
    private const string DICTIONARY_HEADER_MAGIC = "DICT";

    public FileFormatDescriptor FormatDescriptor => new("CGFX", [".fs", ".bcres"]);

    public bool CanImport(Stream stream)
    {
        var buffer = new byte[BUFFER_SIZE];
        var fileHeader = ReadFileHeader(stream, buffer);
        return string.Equals(fileHeader.Magic, FILE_HEADER_MAGIC, StringComparison.Ordinal) &&
               fileHeader.Length == stream.Position;
    }

    public IAsset Import(Stream stream)
    {
        var buffer = new byte[BUFFER_SIZE];
        var fileHeader = ReadFileHeader(stream, buffer);

        if (stream.Position != fileHeader.Length)
        {
            throw new Exception("File header size mismatch");
        }

        stream.Position = fileHeader.Length;
        var dataTable = ReadDataTable(stream, buffer);

        return null;
    }

    private static FileHeader ReadFileHeader(Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, FILE_HEADER_MAGIC.Length);
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

        return new FileHeader(magicString, endianness, length, revision, fileLength, entryCount);
    }

    private DataTable ReadDataTable(Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, DATA_HEADER_MAGIC.Length);
        var magicString = Encoding.ASCII.GetString(buffer);

        if (!string.Equals(magicString, DICTIONARY_HEADER_MAGIC, StringComparison.Ordinal))
            throw new Exception("Invalid dictionary header");

        stream.ReadExactly(buffer, 0, sizeof(int));
        var length = BitConverter.ToInt32(buffer);

        var modelDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var textureDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var lookUpTablesDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var materialsDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var shadersDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var camerasDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var lightsDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var fogsDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var scenesDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var skeletalAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var materialAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var visibilityAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var cameraAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var lightAnimationDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);
        var emittersDictionaryDataEntries = GetDictionaryDataEntry(stream, buffer);

        return new DataTable(length,
            modelDictionaryDataEntries,
            textureDictionaryDataEntries,
            lookUpTablesDictionaryDataEntries,
            materialsDictionaryDataEntries,
            shadersDictionaryDataEntries,
            camerasDictionaryDataEntries,
            lightsDictionaryDataEntries,
            fogsDictionaryDataEntries,
            scenesDictionaryDataEntries,
            skeletalAnimationDictionaryDataEntries,
            materialAnimationDictionaryDataEntries,
            visibilityAnimationDictionaryDataEntries,
            cameraAnimationDictionaryDataEntries,
            lightAnimationDictionaryDataEntries,
            emittersDictionaryDataEntries);
    }

    private static IReadOnlyList<DictionaryDataEntry> GetDictionaryDataEntry(Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, sizeof(uint));
        var entryCount = BitConverter.ToUInt32(buffer, 0);
        var relativeOffset = GetDictionaryDataEntryRelativeOffset(stream, buffer);

        if (entryCount == 0)
            return [];

        var position = stream.Position;
        var dictionaryEntries = GetDictionaryDataEntrySection(stream, buffer, relativeOffset);
        stream.Position = position;

        return dictionaryEntries;
    }

    private static uint GetDictionaryDataEntryRelativeOffset(Stream stream, byte[] buffer)
    {
        var position = (uint)stream.Position;
        stream.ReadExactly(buffer, 0, sizeof(uint));
        var offset = BitConverter.ToUInt32(buffer);

        if (offset != 0)
        {
            offset += position;
        }

        return offset;
    }

    private static IReadOnlyList<DictionaryDataEntry> GetDictionaryDataEntrySection(Stream stream, byte[] buffer,
        uint relativeOffset)
    {
        stream.Seek(relativeOffset, SeekOrigin.Begin);

        stream.ReadExactly(buffer, 0, DICTIONARY_HEADER_MAGIC.Length);
        var magic = Encoding.ASCII.GetString(buffer);

        stream.ReadExactly(buffer, 0, sizeof(uint));
        var length = BitConverter.ToUInt32(buffer);

        stream.ReadExactly(buffer, 0, sizeof(uint));
        var entryCount = BitConverter.ToUInt32(buffer);

        stream.ReadExactly(buffer, 0, sizeof(int));
        var rootNodeReference = BitConverter.ToInt32(buffer);

        stream.ReadExactly(buffer, 0, sizeof(ushort));
        var rootNodeLeft = BitConverter.ToUInt16(buffer);

        stream.ReadExactly(buffer, 0, sizeof(ushort));
        var rootNodeRight = BitConverter.ToUInt16(buffer);

        stream.ReadExactly(buffer, 0, sizeof(uint));
        var rootNodeNameOffset = BitConverter.ToUInt32(buffer);

        stream.ReadExactly(buffer, 0, sizeof(uint));
        var rootNodeNameDataOffset = BitConverter.ToUInt32(buffer);

        var entries = new DictionaryDataEntry[entryCount];

        for (int i = 0; i < entryCount; i++)
        {
            stream.ReadExactly(buffer, 0, sizeof(int));
            var referenceBit = BitConverter.ToInt32(buffer);

            stream.ReadExactly(buffer, 0, sizeof(ushort));
            var nodeLeft = BitConverter.ToUInt16(buffer);

            stream.ReadExactly(buffer, 0, sizeof(ushort));
            var nodeRight = BitConverter.ToUInt16(buffer);

            var nameOffset = GetDictionaryDataEntryRelativeOffset(stream, buffer);
            var dataOffset = GetDictionaryDataEntryRelativeOffset(stream, buffer);

            entries[i] = new DictionaryDataEntry(nameOffset, dataOffset);
        }

        return entries;
    }
}