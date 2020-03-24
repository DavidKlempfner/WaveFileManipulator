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

        public static void ValidateFileContents(byte[] array)
        {
            const int MinSizeOfWavFile = 71;                                                
            if (array.Length < MinSizeOfWavFile)
            {
                throw new ArgumentOutOfRangeException($"File is not large enough for a WAVE file.");
            }

            WaveStringValid(array);
            RiffStringValid(array);
            FmtStringValid(array);
        }

        private static void WaveStringValid(byte[] array)
        {
            var waveString = MetadataGatherer.GetWaveString(array);
            const string Wave = "WAVE";
            if (waveString != Wave)
            {
                ThrowException(Wave);
            }
        }

        private static void RiffStringValid(byte[] array)
        {
            var riffString = MetadataGatherer.GetRiffString(array);
            const string Riff = "RIFF";
            if (riffString != Riff)
            {
                ThrowException(Riff);
            }
        }

        private static void FmtStringValid(byte[] array)
        {
            var fmtString = MetadataGatherer.GetFmtString(array);
            const string Fmt = "fmt";
            if (fmtString != Fmt)
            {
                ThrowException(Fmt);
            }
        }

        private static void ThrowException(string missingString)
        {
            throw new ArgumentException($"{missingString} is not present in file contents.");
        }
    }
}