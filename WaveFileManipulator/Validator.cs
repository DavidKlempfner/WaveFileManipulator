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
            LengthOfFormatDataIs16(array);
        }

        private static void WaveStringValid(byte[] array)
        {
            var waveString = MetadataGatherer.GetWaveString(array);
            const string Wave = "WAVE";
            if (waveString != Wave)
            {
                ThrowMissingException(Wave);
            }
        }

        private static void RiffStringValid(byte[] array)
        {
            var riffString = MetadataGatherer.GetRiffString(array);
            const string Riff = "RIFF";
            if (riffString != Riff)
            {
                ThrowMissingException(Riff);
            }
        }

        private static void FmtStringValid(byte[] array)
        {
            var fmtString = MetadataGatherer.GetFmtString(array);
            const string Fmt = "fmt ";
            if (fmtString != Fmt)
            {
                ThrowMissingException(Fmt);
            }
        }

        private static void LengthOfFormatDataIs16(byte[] array)
        {
            var lengthOfFormatData = MetadataGatherer.GetLengthOfFormatData(array);
            const int ExpectedLengthOfFormatData = 16;
            if (lengthOfFormatData != ExpectedLengthOfFormatData)
            {
                ThrowIncorrectValueException("LengthOfFormat", lengthOfFormatData);
            }
        }

        private static void ThrowMissingException(string missingString)
        {
            throw new ArgumentException($"{missingString} is not present in file contents.");
        }

        private static void ThrowIncorrectValueException(string valueName, int incorrectValue)
        {
            throw new ArgumentException($"{valueName} has an incorrect value of {incorrectValue}.");
        }
    }
}