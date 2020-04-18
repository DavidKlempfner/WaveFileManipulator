/*
https://www.recordingblogs.com/wiki/list-chunk-of-a-wave-file
https://sites.google.com/site/musicgapi/technical-documents/wav-file-format
http://soundfile.sapp.org/doc/WaveFormat/
http://www.topherlee.com/software/pcm-tut-wavformat.html
http://www-mmsp.ece.mcgill.ca/Documents/AudioFormats/WAVE/WAVE.html
Positions Sample Value Description
0 - 3 "RIFF" Marks the file as a riff file. Characters are each 1 byte long.
4 - 7 File size (integer) Size of the overall file - 8 bytes, in bytes (32-bit integer). Typically, you'd fill this in after creation.
8 -11 "WAVE" File Type Header. For our purposes, it always equals "WAVE".
12-15 "fmt " Format chunk marker. Includes trailing null
16-19 16 Length of format data as listed above
20-21 1 Audio format (1 is PCM) - 2 byte integer
22-23 2 Number of Channels - 2 byte integer
24-27 44100 Sample Rate - 32 byte integer. Common values are 44100 (CD), 48000 (DAT). Sample Rate = Number of Samples per second, or Hertz.
28-31 176400 (Sample Rate * BitsPerSample * Channels) / 8.
32-33 4 (BitsPerSample * Channels) / 8.1 - 8 bit mono2 - 8 bit stereo/16 bit mono4 - 16 bit stereo
34-35 16 Bits per sample
36-39 "data" "data" chunk header. Marks the beginning of the data section.
40-43 File size (data) Size of the data section.
Sample values are given above for a 16-bit stereo source.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WaveFileManipulator
{
    public class Manipulator : IManipulator
    {
        public Metadata Metadata { get; private set; }
        private IReverser _reverser;
        private byte[] _forwardsArrayWithOnlyHeaders;
        private byte[] _forwardsArrayWithOnlyAudioData;

        public Manipulator(string filePath, IReverser reverser = null)
        {            
            Validator.ValidateWavFileExtension(filePath);
            var forwardsWavFileStreamByteArray = PopulateBytes(filePath);
            Setup(reverser, forwardsWavFileStreamByteArray);
        }

        public Manipulator(IEnumerable<byte> forwardsWavFileByteCollection, IReverser reverser = null)
        {
            var forwardsWavFileStreamByteArray = forwardsWavFileByteCollection.ToArray();
            Setup(reverser, forwardsWavFileStreamByteArray);
        }

        public byte[] Reverse()
        {            
            var reversedSamples = _reverser.Reverse(Metadata, _forwardsArrayWithOnlyHeaders, _forwardsArrayWithOnlyAudioData);            
            return reversedSamples;
        }

        private byte[] CreateForwardsArrayWithOnlyHeaders(byte[] forwardsWavFileStreamByteArray, int startIndexOfDataChunk)
        {
            byte[] forwardsArrayWithOnlyHeaders = new byte[startIndexOfDataChunk];
            Array.Copy(forwardsWavFileStreamByteArray, 0, forwardsArrayWithOnlyHeaders, 0, startIndexOfDataChunk);
            return forwardsArrayWithOnlyHeaders;
        }

        private byte[] CreateForwardsArrayWithOnlyAudioData(byte[] forwardsWavFileStreamByteArray, int startIndexOfDataChunk)
        {
            byte[] forwardsArrayWithOnlyAudioData = new byte[forwardsWavFileStreamByteArray.Length - startIndexOfDataChunk];
            Array.Copy(forwardsWavFileStreamByteArray, startIndexOfDataChunk, forwardsArrayWithOnlyAudioData, 0, forwardsWavFileStreamByteArray.Length - startIndexOfDataChunk);
            return forwardsArrayWithOnlyAudioData;
        }

        private void Setup(IReverser reverser, byte[] forwardsWavFileStreamByteArray)
        {            
            Metadata = new Metadata(forwardsWavFileStreamByteArray);
            Validator.ValidateFileMinSize(forwardsWavFileStreamByteArray, Metadata);
            if (reverser == null)
            {
                _reverser = new Reverser();
            }
            else
            {
                _reverser = reverser;
            }

            _forwardsArrayWithOnlyHeaders = CreateForwardsArrayWithOnlyHeaders(forwardsWavFileStreamByteArray, Metadata.DataStartIndex);
            _forwardsArrayWithOnlyAudioData = CreateForwardsArrayWithOnlyAudioData(forwardsWavFileStreamByteArray, Metadata.DataStartIndex);
        }

        private byte[] PopulateBytes(string filePath)
        {
            byte[] forwardsWavFileStreamByteArray;
            using (FileStream forwardsWavFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                forwardsWavFileStreamByteArray = new byte[forwardsWavFileStream.Length];
                forwardsWavFileStream.Read(forwardsWavFileStreamByteArray, 0, (int)forwardsWavFileStream.Length);
            }

            return forwardsWavFileStreamByteArray;
        }        
    }
}