using System.Collections.Generic;
using System.Linq;

namespace WaveFileManipulator
{
    public static class SamplesManipulator
    {
        public static byte[] Reverse(int bytesPerSample, ICollection<byte> forwardsArray)
        {
            int length = forwardsArray.Count;
            byte[] reversedArrayWithOnlyAudioData = new byte[length];
            int sampleIdentifier = 0;
            for (int i = 0; i < length; i++)
            {
                if (i != 0 && i % bytesPerSample == 0)
                {
                    sampleIdentifier += 2 * bytesPerSample;
                }
                int index = length - bytesPerSample - sampleIdentifier + i;
                reversedArrayWithOnlyAudioData[i] = forwardsArray.ElementAt(index);
            }
            return reversedArrayWithOnlyAudioData;
        }
    }
}