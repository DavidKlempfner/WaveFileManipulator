using System;
using System.Text;
using System.Linq;

namespace WaveFileManipulator
{
    public static class MetadataGatherer
    {        
        public static Metadata GetMetadata(byte[] array)
        {
            var metadata = new Metadata();
            metadata.ChunkId = ConvertToString(array.SubArray(0, 4));
            metadata.ChunkSize = ConvertToNumber(array.SubArray(4, 4));
            var x = BitConverter.ToUInt32(array.SubArray(4, 4), 0);
            metadata.Format = ConvertToString(array.SubArray(8, 4));
            metadata.SubChunk1Id = ConvertToString(array.SubArray(12, 4));
            metadata.SubChunk1Size = ConvertToNumber(array.SubArray(16, 4));
            metadata.AudioFormat = ConvertToNumber(array.SubArray(20, 2));
            metadata.NumOfChannels = ConvertToNumber(array.SubArray(22, 2));
            metadata.SampleRate = ConvertToNumber(array.SubArray(24, 4));
            metadata.ByteRate = ConvertToNumber(array.SubArray(28, 4));
            metadata.BlockAlign = ConvertToNumber(array.SubArray(32, 2));
            metadata.BitsPerSample = ConvertToNumber(array.SubArray(34, 2));
            metadata.Subchunk2Id = ConvertToString(array.SubArray(36, 4));
            metadata.Subchunk2Size = ConvertToNumber(array.SubArray(40, 4));

            return metadata;
        }

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
            const int StartOWaveString = 8;
            const int LengthOfWaveString = 4;
            var wave = Encoding.UTF8.GetString(array, StartOWaveString, LengthOfWaveString);
            return wave;
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
            const int LengthOfRiffString = 4;
            var fmt = Encoding.UTF8.GetString(array, StartOfRiffString, LengthOfRiffString);
            return fmt;
        }

        /// <summary>
        /// Should be 16
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int GetLengthOfFormatData(byte[] array)
        {
            const int StartOfLengthOfFormatDataValue = 16;
            const int EndOfLengthOfFormatDataValue = 19;
            var lengthOfFormatDataByteArray = GetRelevantBytesIntoNewArray(array, StartOfLengthOfFormatDataValue, EndOfLengthOfFormatDataValue);
            var lengthOfFormatData = BitConverter.ToUInt16(lengthOfFormatDataByteArray, 0);
            return lengthOfFormatData;
        }

        /// <summary>
        /// PCM = 1
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int GetAudioFormat(byte[] array)
        {
            const int StartOfAudioFormat = 20;
            const int EndOfAudioFormat = 21;
            var audioFormatByteArray = GetRelevantBytesIntoNewArray(array, StartOfAudioFormat, EndOfAudioFormat);
            var audioFormat = BitConverter.ToUInt16(audioFormatByteArray, 0);
            return audioFormat;
        }

        /// <summary>
        /// Mono = 1, Stereo = 2
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int GetNumOfChannels(byte[] array)
        {
            const int StartOfNumOfChannels = 22;
            const int EndOfNumOfChannels = 23;
            var numOfChannelsByteArray = GetRelevantBytesIntoNewArray(array, StartOfNumOfChannels, EndOfNumOfChannels);
            var numOfChannels = BitConverter.ToUInt16(numOfChannelsByteArray, 0);
            return numOfChannels;
        }

        /// <summary>
        /// In Hz. 44100 (CD), 48000 (DAT)
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int GetSampleRate(byte[] array)
        {
            const int StartOfSampleRate = 24;
            const int EndOfSampleRate = 27;
            var sampleRateByteArray = GetRelevantBytesIntoNewArray(array, StartOfSampleRate, EndOfSampleRate);
            var sampleRate = BitConverter.ToUInt16(sampleRateByteArray, 0);
            return sampleRate;
        }

        private static byte[] GetRelevantBytesIntoNewArray(byte[] forwardsWavFileStreamByteArray, int startIndex, int endIndex)
        {
            var length = endIndex - startIndex + 1;
            var relevantBytesArray = new byte[length];
            Array.Copy(forwardsWavFileStreamByteArray, startIndex, relevantBytesArray, 0, length);
            return relevantBytesArray;
        }

        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            var subArray = array.Skip(index).Take(length).ToArray();
            return subArray;
        }

        private static int ConvertToNumber(byte[] array)
        {
            int number = BitConverter.ToUInt16(array, 0);            
            return number;
        }

        private static string ConvertToString(byte[] array)
        {
            string text = Encoding.UTF8.GetString(array, 0, array.Length);
            return text;
        }
    }
}