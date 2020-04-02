using System.Linq;

namespace WaveFileManipulator
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            var subArray = array.Skip(index).Take(length).ToArray();
            return subArray;
        }
    }
}