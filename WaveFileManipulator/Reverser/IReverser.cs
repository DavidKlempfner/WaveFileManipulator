namespace WaveFileManipulator
{
    public interface IReverser
    {
        byte[] Reverse(Metadata metadata, byte[] forwardsArrayWithOnlyHeaders, byte[] forwardsArrayWithOnlyAudioData);
    }
}