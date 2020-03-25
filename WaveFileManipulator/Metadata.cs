using System;
using System.Collections.Generic;
using System.Text;

namespace WaveFileManipulator
{
    public class Metadata
    {
        /// <summary>
        /// Endian: Big
        /// Index = 0, Length = 4
        /// Contains the letters "RIFF" in ASCII form.
        /// "RIFX" means the file is in big-endian.
        /// (0x52494646 big-endian form).
        /// </summary>
        public string ChunkId { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 4, Length = 4
        /// 36 + SubChunk2Size, or more precisely:
        /// 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size).
        /// ie fileSize - 8 bytes.
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// Endian: Big
        /// Index = 8, Length = 4
        /// "WAVE"
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Endian: Big
        /// Index = 12, Length = 4
        /// "fmt "
        /// (0x666d7420 big-endian form).
        /// </summary>
        public string SubChunk1Id { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 16, Length = 4
        /// 16 for PCM. This is the size of the rest of the Subchunk which follows this number.
        /// </summary>
        public int SubChunk1Size { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 20, Length = 2
        /// PCM = 1
        /// </summary>
        public int AudioFormat { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 22, Length = 2
        /// Mono = 1, Stereo = 2
        /// </summary>
        public int NumOfChannels { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 24, Length = 4
        /// 8000, 44100, etc
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 28, Length = 4
        /// SampleRate * NumChannels * BitsPerSample/8
        /// </summary>
        public int ByteRate { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 32, Length = 2
        /// NumChannels * BitsPerSample/8
        /// The number of bytes for one sample including all channels.
        /// </summary>
        public int BlockAlign { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 34, Length = 2
        /// 8 bits = 8, 16 bits = 16, etc.
        /// </summary>
        public int BitsPerSample { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 36, Length = 4
        /// "data"
        /// </summary>
        public string Subchunk2Id { get; set; }

        /// <summary>
        /// Endian: Little
        /// Index = 40, Length = 4
        /// NumSamples * NumChannels * BitsPerSample/8
        /// This is the number of bytes in the data.
        /// </summary>
        public int Subchunk2Size { get; set; }
    }
}

//TODO:
//The default byte ordering assumed for WAVE data files is little-endian.Files written using the big-endian byte ordering scheme have the identifier RIFX instead of RIFF.