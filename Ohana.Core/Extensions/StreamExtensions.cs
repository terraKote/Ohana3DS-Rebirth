using System.Text;

namespace Ohana.Core.Extensions;

public static class StreamExtensions
{
    public static string ReadString(this Stream stream, int length, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, length);
        return Encoding.ASCII.GetString(buffer);
    }

    public static int ReadInt32(this Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, sizeof(int));
        return BitConverter.ToInt32(buffer);
    }
    
    public static uint ReadUInt32(this Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, sizeof(uint));
        return BitConverter.ToUInt32(buffer);
    }

    public static ushort ReadUInt16(this Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, sizeof(ushort));
        return BitConverter.ToUInt16(buffer);
    }
}