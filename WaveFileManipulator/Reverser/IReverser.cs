namespace WaveFileManipulator
{
    public interface IReverser
    {
        byte[] Reverse(Metadata metadata, byte[] forwardsWavFileStreamByteArray);
    }
}