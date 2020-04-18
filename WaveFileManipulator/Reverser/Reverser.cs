using System;

namespace WaveFileManipulator
{
    internal class Reverser : IReverser
    {        
        public byte[] Reverse(Metadata metadata, byte[] forwardsArrayWithOnlyHeaders, byte[] forwardsArrayWithOnlyAudioData)
        {            
            const int BitsPerByte = 8;
            int bytesPerSample = metadata.BitsPerSample.Value / BitsPerByte;
            byte[] reversedArrayWithOnlyAudioData = SamplesManipulator.Reverse(bytesPerSample, forwardsArrayWithOnlyAudioData);
            byte[] reversedWavFileStreamByteArray = CombineArrays(forwardsArrayWithOnlyHeaders, reversedArrayWithOnlyAudioData);

            return reversedWavFileStreamByteArray;
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