using System;
using System.Text;

namespace WaveFileManipulator
{
    public class Metadata
    {
        public Metadata(byte[] array)
        {
            ChunkId = ConvertToString(array.SubArray(0, 4));
            ChunkSize = ConvertToUInt(array.SubArray(4, 4));
            Format = ConvertToString(array.SubArray(8, 4));
            SubChunk1Id = ConvertToString(array.SubArray(12, 4));
            SubChunk1Size = ConvertToUInt(array.SubArray(16, 4));
            AudioFormat = ConvertToUShort(array.SubArray(20, 2));
            NumOfChannels = ConvertToUShort(array.SubArray(22, 2));
            SampleRate = ConvertToUInt(array.SubArray(24, 4));
            ByteRate = ConvertToUInt(array.SubArray(28, 4));
            BlockAlign = ConvertToUShort(array.SubArray(32, 2));
            BitsPerSample = ConvertToUShort(array.SubArray(34, 2));
            Subchunk2Id = ConvertToString(array.SubArray(36, 4));
            Subchunk2Size = ConvertToUInt(array.SubArray(40, 4));
        }
        /// <summary>
        /// Endian: Big.
        /// Index = 0, Length = 4.
        /// Contains the letters "RIFF" in ASCII form.
        /// "RIFX" means the file is in big-endian.
        /// (0x52494646 big-endian form).
        /// </summary>
        public string ChunkId { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 4, Length = 4.
        /// 36 + SubChunk2Size, or more precisely:
        /// 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size).
        /// ie fileSize - 8 bytes.
        /// </summary>
        public uint ChunkSize { get; private set; }

        /// <summary>
        /// Endian: Big.
        /// Index = 8, Length = 4.
        /// "WAVE".
        /// </summary>
        public string Format { get; private set; }

        /// <summary>
        /// Endian: Big.
        /// Index = 12, Length = 4.
        /// "fmt ".
        /// (0x666d7420 big-endian form).
        /// </summary>
        public string SubChunk1Id { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 16, Length = 4.
        /// 16 for PCM. This is the size of the rest of the Subchunk (format data) which follows this number.
        /// </summary>
        public uint SubChunk1Size { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 20, Length = 2.
        /// PCM = 1.
        /// </summary>
        public ushort AudioFormat { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 22, Length = 2.
        /// Mono = 1, Stereo = 2.
        /// </summary>
        public ushort NumOfChannels { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 24, Length = 4.
        /// 8000, 44100, etc.
        /// </summary>
        public uint SampleRate { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 28, Length = 4.
        /// SampleRate * NumChannels * BitsPerSample/8.
        /// </summary>
        public uint ByteRate { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 32, Length = 2.
        /// NumChannels * BitsPerSample/8.
        /// The number of bytes for one sample including all channels.
        /// </summary>
        public ushort BlockAlign { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 34, Length = 2.
        /// 8 bits = 8, 16 bits = 16, etc.
        /// </summary>
        public ushort BitsPerSample { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 36, Length = 4.
        /// "data".
        /// </summary>
        public string Subchunk2Id { get; private set; }

        /// <summary>
        /// Endian: Little.
        /// Index = 40, Length = 4.
        /// NumSamples * NumChannels * BitsPerSample/8.
        /// This is the number of bytes in the data.
        /// </summary>
        public uint Subchunk2Size { get; private set; }

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