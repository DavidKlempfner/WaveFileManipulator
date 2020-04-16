using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WaveFileManipulator
{
    public class Metadata
    {
        const string DataText = "data";
        
        public readonly IReadOnlyDictionary<string, string> Info;

        public Metadata(byte[] array)
        {
            ArraySize = array.Length;

            NumOfSamples = ArraySize - DataStartIndex;

            var numOfChannelsArray = array.SubArray(NumOfChannels.StartIndex, NumOfChannels.Length);
            NumOfChannels = new NumOfChannels(Converters.ConvertToUShort(numOfChannelsArray));

            var sampleRateArray = array.SubArray(SampleRate.StartIndex, SampleRate.Length);
            SampleRate = new SampleRate(Converters.ConvertToUInt(sampleRateArray));

            var bitsPerSampleArray = array.SubArray(BitsPerSample.StartIndex, BitsPerSample.Length);
            BitsPerSample = new BitsPerSample(Converters.ConvertToUShort(bitsPerSampleArray));

            var chunkIdArray = array.SubArray(ChunkId.StartIndex, ChunkId.Length);
            const string chunkIdExpectedValue = "RIFF";
            ChunkId = new ChunkId(Converters.ConvertToString(chunkIdArray), chunkIdExpectedValue);

            var chunkSizeArray = array.SubArray(ChunkSize.StartIndex, ChunkSize.Length);
            var chunkSizeExpectedValue = ArraySize - 8;
            ChunkSize = new ChunkSize(Converters.ConvertToUInt(chunkSizeArray), (uint)chunkSizeExpectedValue);

            var formatArray = array.SubArray(Format.StartIndex, Format.Length);
            const string formatArrayExpectedValue = "WAVE";
            Format = new Format(Converters.ConvertToString(formatArray), formatArrayExpectedValue);

            var subChunk1IdArray = array.SubArray(SubChunk1Id.StartIndex, SubChunk1Id.Length);
            const string subChunk1IdExpectedValue = "fmt ";
            SubChunk1Id = new SubChunk1Id(Converters.ConvertToString(subChunk1IdArray), subChunk1IdExpectedValue);

            var subChunk1SizeArray = array.SubArray(SubChunk1Size.StartIndex, SubChunk1Size.Length);
            SubChunk1Size = new SubChunk1Size(Converters.ConvertToUInt(subChunk1SizeArray));

            var audioFormatArray = array.SubArray(AudioFormat.StartIndex, AudioFormat.Length);
            AudioFormat = new AudioFormat(Converters.ConvertToUShort(audioFormatArray));

            var byteRateArray = array.SubArray(ByteRate.StartIndex, ByteRate.Length);
            uint byteRateExpectedValue = SampleRate.Value * NumOfChannels.Value * BitsPerSample.Value / 8;
            ByteRate = new ByteRate(Converters.ConvertToUInt(byteRateArray), byteRateExpectedValue);

            var blockAlignArray = array.SubArray(BlockAlign.StartIndex, BlockAlign.Length);
            var blockAlignExpectedValue = NumOfChannels.Value * BitsPerSample.Value / 8;
            BlockAlign = new BlockAlign(Converters.ConvertToUShort(blockAlignArray), (ushort)blockAlignExpectedValue);

            var subChunk2IdArray = array.SubArray(SubChunk2Id.StartIndex, SubChunk2Id.Length);
            SubChunk2Id = new SubChunk2Id(Converters.ConvertToString(subChunk2IdArray));

            var subChunk2SizeArray = array.SubArray(SubChunk2Size.StartIndex, SubChunk2Size.Length);
            var subChunk2SizeExpectedValue = ArraySize - SubChunk2Size.StartIndex - SubChunk2Size.Length;
            SubChunk2Size = new SubChunk2Size(Converters.ConvertToUInt(subChunk2SizeArray), (uint)subChunk2SizeExpectedValue);

            DataStartIndex = GetDataStartIndex(array);

            //https://www.recordingblogs.com/wiki/list-chunk-of-a-wave-file
            var listOfKeys = new List<string>
            {
                "IARL", "IART", "ICMS", "ICMT", "ICOP", 
                "ICRD", "ICRP", "IDIM", "IDPI", "IENG", 
                "IGNR", "IKEY", "ILGT", "IMED", "INAM", 
                "IPLT", "IPRD", "ISBJ", "ISFT", "ISRC", 
                "ISRF", "ITCH"
            };
            var dictionary = PopulateInfo(listOfKeys, array);
            Info = new ReadOnlyDictionary<string, string>(dictionary);
        }

        private Dictionary<string, string> PopulateInfo(List<string> keys, byte[] array)
        {
            const string infoIdText = "INFO";
            var infoIdStartIndex = ArraySearcher.GetStartIndexOfText(array, infoIdText);

            const int notFoundIndicator = -1;
            Dictionary<string, string> info = new Dictionary<string, string>();
            if (infoIdStartIndex != notFoundIndicator)
            {
                var infoDataStartIndex = infoIdStartIndex + infoIdText.Length;
                var arrayFromInfoDataStartIndex = array.SubArray(infoDataStartIndex, array.Length - infoDataStartIndex);

                info = ArraySearcher.GetKeysAndValues(keys, arrayFromInfoDataStartIndex);
            }
            return info;
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
        public ChunkSize ChunkSize { get; private set; }

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

        /// <summary>
        /// Endian: Little.
        /// Index = 28, Length = 4.
        /// SampleRate * NumChannels * BitsPerSample/8
        /// uint
        /// </summary>
        public ByteRate ByteRate { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 32, Length = 2.
        /// NumChannels * BitsPerSample/8.
        /// The number of bytes for one sample including all channels.
        /// ushort
        /// </summary>
        public BlockAlign BlockAlign { get; private set; }

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
        public SubChunk2Size SubChunk2Size { get; private set; }

        public int DataStartIndex { get; private set; }

        public int ArraySize { get; private set; }

        public int NumOfSamples { get; private set; }

        private int GetDataStartIndex(byte[] array)
        {
            var indexAfterSubChunk2Size = SubChunk2Size.StartIndex + SubChunk2Size.Length;
            int dataStartIndex;
            if (SubChunk2Id.Value == DataText) //Expected start index for "data"
            {
                dataStartIndex = indexAfterSubChunk2Size;
            }
            else
            {
                const int chunkSizeLength = 4;
                var startIndexOfText = ArraySearcher.GetStartIndexOfText(array, DataText);
                if (startIndexOfText == -1)
                {
                    throw new ArgumentException($"{DataText} not found.");
                }
                dataStartIndex = startIndexOfText + DataText.Length + chunkSizeLength;
            }
            return dataStartIndex;
        }
    }
}