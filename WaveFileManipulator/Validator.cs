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

        public static void ValidateFileContents(byte[] array, Metadata metadata)
        {
            const int MinSizeOfWavFile = 71;                                                
            if (array.Length < MinSizeOfWavFile)
            {
                throw new ArgumentOutOfRangeException($"File is not large enough for a WAVE file.");
            }

            WaveStringValid(metadata.Format.Value);
            RiffStringValid(metadata.ChunkId.Value);
            FmtStringValid(metadata.SubChunk1Id.Value);
            LengthOfFormatDataIs16(metadata.SubChunk1Size.Value);
        }

        private static void WaveStringValid(string waveString)
        {
            const string Wave = "WAVE";
            if (waveString != Wave)
            {
                ThrowMissingException(Wave);
            }
        }

        private static void RiffStringValid(string riffString)
        {
            const string Riff = "RIFF";
            if (riffString != Riff)
            {
                ThrowMissingException(Riff);
            }
        }

        private static void FmtStringValid(string fmtString)
        {
            const string Fmt = "fmt ";
            if (fmtString != Fmt)
            {
                ThrowMissingException(Fmt);
            }
        }

        private static void LengthOfFormatDataIs16(uint lengthOfFormatData)
        {
            const uint ExpectedLengthOfFormatData = 16;
            if (lengthOfFormatData != ExpectedLengthOfFormatData)
            {
                ThrowIncorrectValueException("LengthOfFormat", lengthOfFormatData);
            }
        }

        private static void ThrowMissingException(string missingString)
        {
            throw new ArgumentException($"{missingString} is not present in file contents.");
        }

        private static void ThrowIncorrectValueException(string valueName, uint incorrectValue)
        {
            throw new ArgumentException($"{valueName} has an incorrect value of {incorrectValue}.");
        }

        //TODO: validate all other values.
    }
}