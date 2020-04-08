using System;
using System.IO;

namespace WaveFileManipulator
{
    public static class Validator
    {
        public static void ValidateWavFileExtension(string filePath)
        {
            const string WaveExtension = ".wav";
            if (Path.GetExtension(filePath) != WaveExtension)
            {
                throw new ArgumentException($"File extension must be {WaveExtension}.");
            }
        }

        public static void ValidateFileMinSize(byte[] array, Metadata metadata)
        {
            const int MinSizeOfWavFile = SubChunk2Size.StartIndex + SubChunk2Size.Length + 1;                                                
            if (array.Length < MinSizeOfWavFile)
            {
                throw new ArgumentOutOfRangeException($"File is not large enough for a WAVE file.");
            }
        }
    }
}