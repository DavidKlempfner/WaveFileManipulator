using System;
using System.Text;

namespace WaveFileManipulator
{
    public static class MetadataGatherer
    {
        internal static ushort GetBitsPerSample(byte[] array)
        {
            const int BitsPerSampleStartIndex = 34;
            const int BitsPerSampleEndIndex = 35;
            var bitsPerSampleByteArray = GetRelevantBytesIntoNewArray(array, BitsPerSampleStartIndex, BitsPerSampleEndIndex);
            var bitsPerSample = BitConverter.ToUInt16(bitsPerSampleByteArray, 0);
            return bitsPerSample;
        }

        public static string GetWaveString(byte[] array)
        {
            const int StartOfRiffString = 8;
            const int LengthOfRiffString = 4;
            var riff = Encoding.UTF8.GetString(array, StartOfRiffString, LengthOfRiffString);
            return riff;
        }

        public static string GetRiffString(byte[] array)
        {
            const int StartOfRiffString = 0;
            const int LengthOfRiffString = 4;
            var riff = Encoding.UTF8.GetString(array, StartOfRiffString, LengthOfRiffString);
            return riff;
        }

        public static string GetFmtString(byte[] array)
        {
            const int StartOfRiffString = 12;
            const int LengthOfRiffString = 3;
            var fmt = Encoding.UTF8.GetString(array, StartOfRiffString, LengthOfRiffString);
            return fmt;
        }

        private static byte[] GetRelevantBytesIntoNewArray(byte[] forwardsWavFileStreamByteArray, int startIndex, int endIndex)
        {
            var length = endIndex - startIndex + 1;
            var relevantBytesArray = new byte[length];
            Array.Copy(forwardsWavFileStreamByteArray, startIndex, relevantBytesArray, 0, length);
            return relevantBytesArray;
        }
    }
}