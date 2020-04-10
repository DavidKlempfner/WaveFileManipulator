﻿using System;
using System.Text;

namespace WaveFileManipulator
{
    public class Metadata
    {
        const string DataText = "data";
        public Metadata(byte[] array)
        {
            ArraySize = array.Length;

            NumOfSamples = ArraySize - DataStartIndex;

            var numOfChannelsArray = array.SubArray(NumOfChannels.StartIndex, NumOfChannels.Length);
            NumOfChannels = new NumOfChannels(ConvertToUShort(numOfChannelsArray));

            var sampleRateArray = array.SubArray(SampleRate.StartIndex, SampleRate.Length);
            SampleRate = new SampleRate(ConvertToUInt(sampleRateArray));

            var bitsPerSampleArray = array.SubArray(BitsPerSample.StartIndex, BitsPerSample.Length);
            BitsPerSample = new BitsPerSample(ConvertToUShort(bitsPerSampleArray));

            var chunkIdArray = array.SubArray(ChunkId.StartIndex, ChunkId.Length);
            ChunkId = new ChunkId(ConvertToString(chunkIdArray));

            var chunkSizeArray = array.SubArray(ChunkSize.StartIndex, ChunkSize.Length);
            uint chunkSizeExpectedValue = ArraySize - 8;
            ChunkSize = new ChunkSize(ConvertToUInt(chunkSizeArray), chunkSizeExpectedValue);

            var formatArray = array.SubArray(Format.StartIndex, Format.Length);
            Format = new Format(ConvertToString(formatArray));

            var subChunk1IdArray = array.SubArray(SubChunk1Id.StartIndex, SubChunk1Id.Length);
            SubChunk1Id = new SubChunk1Id(ConvertToString(subChunk1IdArray));

            var subChunk1SizeArray = array.SubArray(SubChunk1Size.StartIndex, SubChunk1Size.Length);
            SubChunk1Size = new SubChunk1Size(ConvertToUInt(subChunk1SizeArray));

            var audioFormatArray = array.SubArray(AudioFormat.StartIndex, AudioFormat.Length);
            AudioFormat = new AudioFormat(ConvertToUShort(audioFormatArray));

            var byteRateArray = array.SubArray(ByteRate.StartIndex, ByteRate.Length);
            uint byteRateExpectedValue = SampleRate.Value * NumOfChannels.Value * BitsPerSample.Value / 8;
            ByteRate = new ByteRate(ConvertToUInt(byteRateArray), byteRateExpectedValue);

            var blockAlignArray = array.SubArray(BlockAlign.StartIndex, BlockAlign.Length);
            BlockAlign = new BlockAlign(ConvertToUShort(blockAlignArray));

            var subChunk2IdArray = array.SubArray(SubChunk2Id.StartIndex, SubChunk2Id.Length);
            SubChunk2Id = new SubChunk2Id(ConvertToString(subChunk2IdArray));

            var subChunk2SizeArray = array.SubArray(SubChunk2Size.StartIndex, SubChunk2Size.Length);
            SubChunk2Size = new SubChunk2Size(ConvertToUInt(subChunk2SizeArray));

            DataStartIndex = GetDataStartIndex(array);
        }

        /// <summary>
        /// Endian: Big.
        /// Index = 0, Length = 4.
        /// Contains the letters "RIFF" in ASCII form.
        /// "RIFX" means the file is in big-endian.
        /// (0x52494646 big-endian form).
        /// string
        /// </summary>
        public ChunkId ChunkId { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 4, Length = 4.
        /// 36 + SubChunk2Size, or more precisely:
        /// 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size).
        /// ie fileSize - 8 bytes.
        /// uint
        /// </summary>
        private ChunkSize _chunkSize;
        public ChunkSize ChunkSize
        {
            get { return _chunkSize; }
            private set
            {
                var expectedValue = ArraySize - 8;
                if (expectedValue == value.Value)
                {
                    _chunkSize = value;
                }
                else
                {
                    ThrowOutOfRangeException(expectedValue, value.Value, nameof(ChunkSize));
                }
            }
        }

        /// <summary>
        /// Endian: Big.
        /// Index = 8, Length = 4.
        /// "WAVE".
        /// string
        /// </summary>
        public Format Format { get; private set; }

        /// <summary>
        /// Endian: Big.
        /// Index = 12, Length = 4.
        /// "fmt ".
        /// (0x666d7420 big-endian form).
        /// string
        /// </summary>
        public SubChunk1Id SubChunk1Id { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 16, Length = 4.
        /// 16 for PCM. This is the size of the rest of the SubChunk (format data) which follows this number.
        /// uint
        /// </summary>
        public SubChunk1Size SubChunk1Size { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 20, Length = 2.
        /// PCM = 1.
        /// ushort
        /// </summary>
        public AudioFormat AudioFormat { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 22, Length = 2.
        /// Mono = 1, Stereo = 2.
        /// ushort
        /// </summary>
        public NumOfChannels NumOfChannels { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 24, Length = 4.
        /// 8000, 44100, etc.
        /// uint
        /// </summary>
        public SampleRate SampleRate { get; private set; }
        public ByteRate ByteRate { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 32, Length = 2.
        /// NumChannels * BitsPerSample/8.
        /// The number of bytes for one sample including all channels.
        /// ushort
        /// </summary>
        private BlockAlign _blockAlign;
        public BlockAlign BlockAlign
        {
            get { return _blockAlign; }
            private set
            {
                var expectedValue = NumOfChannels.Value * BitsPerSample.Value / 8;
                if (expectedValue == value.Value)
                {
                    _blockAlign = value;
                }
                else
                {
                    ThrowOutOfRangeException(expectedValue, value.Value, nameof(BlockAlign));
                }
            }
        }

        /// <summary>
        /// Endian: Little.
        /// Index = 34, Length = 2.
        /// 8 bits = 8, 16 bits = 16, etc.
        /// ushort
        /// </summary>
        public BitsPerSample BitsPerSample { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 36, Length = 4.
        /// "data".
        /// string
        /// </summary>
        public SubChunk2Id SubChunk2Id { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 40, Length = 4.
        /// NumOfSamples * NumOfChannels * BitsPerSample/8. This is not correct.
        /// This is the number of bytes in the data.
        /// uint
        /// </summary>
        private SubChunk2Size _subChunk2Size;
        public SubChunk2Size SubChunk2Size
        {
            get { return _subChunk2Size; }
            private set
            {
                var expectedValue = ArraySize - SubChunk2Size.StartIndex - SubChunk2Size.Length;
                if (expectedValue == value.Value || SubChunk2Id.Value == "LIST")
                {
                    _subChunk2Size = value;
                }
                else
                {
                    //ThrowOutOfRangeException(expectedValue, value.Value, nameof(SubChunk2Size));
                }
            }
        }

        public int DataStartIndex { get; private set; }

        public int ArraySize { get; private set; }

        public int NumOfSamples { get; private set; }

        private int GetDataStartIndex(byte[] array)
        {
            var indexAfterSubChunk2Size = SubChunk2Size.StartIndex + SubChunk2Size.Length;
            int dataStartIndex;
            if (SubChunk2Id.Value == DataText) //Normal start index for "data"
            {
                dataStartIndex = indexAfterSubChunk2Size;
            }
            else
            {
                //dataStartIndex = SearchForStartIndexAfterHeader(array.SubArray(SubChunk2Id.StartIndex, array.Length - SubChunk2Id.StartIndex));
                dataStartIndex = SearchForStartIndex(array);
            }
            return dataStartIndex;
        }
        
        private int SearchForStartIndex(byte[] array)
        {
            const byte dAscii = 100;
            const byte aAscii = 97;
            const byte tAscii = 116;

            const int lengthOfSizeValue = 4;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == dAscii)
                {
                    var secondCharIndex = i + 1;
                    var thirdCharIndex = i + 2;
                    var fourthCharIndex = i + 3;
                    if (array[secondCharIndex] == aAscii && array[thirdCharIndex] == tAscii && array[fourthCharIndex] == aAscii)
                    {
                        var startIndex = fourthCharIndex + lengthOfSizeValue + 1;
                        return startIndex;
                    }
                }
            }
            //const int lengthOfChunkId = 4;
            //const int lengthOfValue = 4;                        

            //string chunkId = ConvertToString(array.SubArray(0, lengthOfChunkId));
            //if (chunkId == DataText)
            //{
            //    int startIndex = ArraySize - array.Length + lengthOfChunkId + lengthOfValue;
            //    return startIndex;
            //}
            //var chunkSize = (int)ConvertToUInt(array.SubArray(lengthOfChunkId, lengthOfValue));
            //int numOfBytesToSkip = lengthOfChunkId + chunkSize + lengthOfValue; //"AnId", chunkSize (eg 26 bytes), 
            //return SearchForStartIndexAfterHeader(array.SubArray(numOfBytesToSkip, array.Length - numOfBytesToSkip));

            throw new ArgumentException($"Could not find {DataText} start index.");
        }

        private string ConvertToString(byte[] array)
        {
            return Encoding.UTF8.GetString(array, 0, array.Length);
        }

        private uint ConvertToUInt(byte[] array)
        {
            return BitConverter.ToUInt32(array, 0);
        }

        private ushort ConvertToUShort(byte[] array)
        {
            return BitConverter.ToUInt16(array, 0);
        }

        private void ThrowOutOfRangeException(object expectedValue, object actualValue, string typeName)
        {
            throw new ArgumentOutOfRangeException(typeName, $"Expected {expectedValue} but received {actualValue}.");
        }
    }    
}

//TODO:
//The default byte ordering assumed for WAVE data files is little-endian.Files written using the big-endian byte ordering scheme have the identifier RIFX instead of RIFF.