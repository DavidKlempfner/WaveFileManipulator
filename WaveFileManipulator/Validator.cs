using System;
using System.IO;
using System.Text;

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

        public static void ValidateFileTypeHeader(byte[] array)
        {
            const int StartOfFileTypeHeader = 8;
            const int EndOfFileTypeHeader = 11;
            var fileTypeHeader = Encoding.UTF8.GetString(array, StartOfFileTypeHeader, EndOfFileTypeHeader);
            if (fileTypeHeader != "WAVE")
            {
                throw new ArgumentException("File's contents do not comply with the WAVE file format");
            }
        }
    }
}