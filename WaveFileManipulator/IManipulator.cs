using System.Collections.Generic;

namespace WaveFileManipulator
{
    public interface IManipulator
    {
        byte[] Reverse(string forwardsWavFilePath);
        byte[] Reverse(IEnumerable<byte> forwardsWavFileByteCollection);
    }
}