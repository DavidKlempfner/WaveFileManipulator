using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace WaveFileManipulator
{
    public class Metadata
    {
        const string DataText = "data";
        
        /// <summary>
        /// Values are from https://www.recordingblogs.com/wiki/list-chunk-of-a-wave-file
        /// </summary>
        private readonly Dictionary<string, string> _info = new Dictionary<string, string>
        {
            { "IARL", "" }, { "IART", "" }, { "ICMS", "" }, { "ICMT", "" }, { "ICOP", "" },
            { "ICRD", "" }, { "ICRP", "" }, { "IDIM", "" }, { "IDPI", "" }, { "IENG", "" },
            { "IGNR", "" }, { "IKEY", "" }, { "ILGT", "" }, { "IMED", "" }, { "INAM", "" },
            { "IPLT", "" }, { "IPRD", "" }, { "ISBJ", "" }, { "ISFT", "" }, { "ISRC", "" }, 
            { "ISRF", "" }, { "ITCH", "" }
        };
        public readonly IReadOnlyDictionary<string, string> Info;

        public Metadata(byte[] array)
        {
            //LIST
            //Info ID(4 byte ASCII text) for information 1
            //Size of text 1
            //Text 1
            //Info ID(4 byte ASCII text) for information 2
            //Size of text 2
            //Text 2            

            ArraySize = array.Length;

            NumOfSamples = ArraySize - DataStartIndex;

            var numOfChannelsArray = array.SubArray(NumOfChannels.StartIndex, NumOfChannels.Length);
            NumOfChannels = new NumOfChannels(ConvertToUShort(numOfChannelsArray));

            var sampleRateArray = array.SubArray(SampleRate.StartIndex, SampleRate.Length);
            SampleRate = new SampleRate(ConvertToUInt(sampleRateArray));

            var bitsPerSampleArray = array.SubArray(BitsPerSample.StartIndex, BitsPerSample.Length);
            BitsPerSample = new BitsPerSample(ConvertToUShort(bitsPerSampleArray));

            var chunkIdArray = array.SubArray(ChunkId.StartIndex, ChunkId.Length);
            const string chunkIdExpectedValue = "RIFF";
            ChunkId = new ChunkId(ConvertToString(chunkIdArray), chunkIdExpectedValue);

            var chunkSizeArray = array.SubArray(ChunkSize.StartIndex, ChunkSize.Length);
            var chunkSizeExpectedValue = ArraySize - 8;
            ChunkSize = new ChunkSize(ConvertToUInt(chunkSizeArray), (uint)chunkSizeExpectedValue);

            var formatArray = array.SubArray(Format.StartIndex, Format.Length);
            const string formatArrayExpectedValue = "WAVE";
            Format = new Format(ConvertToString(formatArray), formatArrayExpectedValue);

            var subChunk1IdArray = array.SubArray(SubChunk1Id.StartIndex, SubChunk1Id.Length);
            const string subChunk1IdExpectedValue = "fmt ";
            SubChunk1Id = new SubChunk1Id(ConvertToString(subChunk1IdArray), subChunk1IdExpectedValue);

            var subChunk1SizeArray = array.SubArray(SubChunk1Size.StartIndex, SubChunk1Size.Length);
            SubChunk1Size = new SubChunk1Size(ConvertToUInt(subChunk1SizeArray));

            var audioFormatArray = array.SubArray(AudioFormat.StartIndex, AudioFormat.Length);
            AudioFormat = new AudioFormat(ConvertToUShort(audioFormatArray));

            var byteRateArray = array.SubArray(ByteRate.StartIndex, ByteRate.Length);
            uint byteRateExpectedValue = SampleRate.Value * NumOfChannels.Value * BitsPerSample.Value / 8;
            ByteRate = new ByteRate(ConvertToUInt(byteRateArray), byteRateExpectedValue);

            var blockAlignArray = array.SubArray(BlockAlign.StartIndex, BlockAlign.Length);
            var blockAlignExpectedValue = NumOfChannels.Value * BitsPerSample.Value / 8;
            BlockAlign = new BlockAlign(ConvertToUShort(blockAlignArray), (ushort)blockAlignExpectedValue);

            var subChunk2IdArray = array.SubArray(SubChunk2Id.StartIndex, SubChunk2Id.Length);
            SubChunk2Id = new SubChunk2Id(ConvertToString(subChunk2IdArray));

            var subChunk2SizeArray = array.SubArray(SubChunk2Size.StartIndex, SubChunk2Size.Length);
            var subChunk2SizeExpectedValue = ArraySize - SubChunk2Size.StartIndex - SubChunk2Size.Length;
            SubChunk2Size = new SubChunk2Size(ConvertToUInt(subChunk2SizeArray), (uint)subChunk2SizeExpectedValue);

            DataStartIndex = GetDataStartIndex(array);

            //PopulateInfo(_info, array);
            Info = new ReadOnlyDictionary<string, string>(_info);
        }

        private void PopulateInfo(Dictionary<string, string> info, byte[] array)
        {
            int infoStartIndex;
            
            //for (int i = 0; i < length; i++)
            //{

            //}
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
                var startIndexOfText = TextFinder.GetStartIndexOfText(array, DataText);
                if (startIndexOfText == -1)
                {
                    throw new ArgumentException($"{DataText} not found.");
                }
                dataStartIndex = startIndexOfText + DataText.Length + chunkSizeLength;                
            }
            return dataStartIndex;
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
    }
}

//TODO:
//The default byte ordering assumed for WAVE data files is little-endian.Files written using the big-endian byte ordering scheme have the identifier RIFX instead of RIFF.