using System;

namespace WaveFileManipulator
{
    internal class Reverser : IReverser
    {
        public byte[] Reverse(Metadata metadata, byte[] forwardsWavFileStreamByteArray)
        {
            byte[] forwardsArrayWithOnlyHeaders = CreateForwardsArrayWithOnlyHeaders(forwardsWavFileStreamByteArray, metadata.DataStartIndex);
            byte[] forwardsArrayWithOnlyAudioData = CreateForwardsArrayWithOnlyAudioData(forwardsWavFileStreamByteArray, metadata.DataStartIndex);

            const int BitsPerByte = 8;
            int bytesPerSample = metadata.BitsPerSample.Value / BitsPerByte;
            byte[] reversedArrayWithOnlyAudioData = SamplesManipulator.Reverse(bytesPerSample, forwardsArrayWithOnlyAudioData);
            byte[] reversedWavFileStreamByteArray = CombineArrays(forwardsArrayWithOnlyHeaders, reversedArrayWithOnlyAudioData);

            return reversedWavFileStreamByteArray;
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

        private byte[] CombineArrays(byte[] forwardsArrayWithOnlyHeaders, byte[] reversedArrayWithOnlyAudioData)
        {
            byte[] reversedWavFileStreamByteArray = new byte[forwardsArrayWithOnlyHeaders.Length + reversedArrayWithOnlyAudioData.Length];
            Array.Copy(forwardsArrayWithOnlyHeaders, reversedWavFileStreamByteArray, forwardsArrayWithOnlyHeaders.Length);
            Array.Copy(reversedArrayWithOnlyAudioData, 0, reversedWavFileStreamByteArray, forwardsArrayWithOnlyHeaders.Length, reversedArrayWithOnlyAudioData.Length);
            return reversedWavFileStreamByteArray;
        }
    }
}