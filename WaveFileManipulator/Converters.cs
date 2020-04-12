using System;
using System.Text;

namespace WaveFileManipulator
{
    public static class Converters
    {
        public static string ConvertToString(byte[] array)
        {
            return Encoding.UTF8.GetString(array, 0, array.Length);
        }

        public static uint ConvertToUInt(byte[] array)
        {
            return BitConverter.ToUInt32(array, 0);
        }

        public static ushort ConvertToUShort(byte[] array)
        {
            return BitConverter.ToUInt16(array, 0);
        }
    }
}