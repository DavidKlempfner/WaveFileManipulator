using System;
using System.Linq;

namespace WaveFileManipulator
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            if (index >= array.Length)
            {
                throw new ArgumentException($"{index} is >= array length {array.Length}.", "index");
            }
            if (index + length > array.Length)
            {
                throw new ArgumentException($"{nameof(index)} {index} + {nameof(length)} {length} is > array length {array.Length}.");
            }
            var subArray = array.Skip(index).Take(length).ToArray();
            return subArray;
        }
    }
}