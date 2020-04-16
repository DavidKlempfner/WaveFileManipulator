using System.Collections.Generic;

namespace WaveFileManipulator
{
    public static class ArraySearcher
    {
        public static int GetStartIndexOfText(byte[] array, string text)
        {
            for (int i = 0; i < array.Length; i++)
            {
                var notEnoughSpaceLeftForText = i > array.Length - text.Length;
                if (notEnoughSpaceLeftForText)
                {
                    break;
                }
                var foundFirstChar = array[i] == text[0];
                if (foundFirstChar)
                {
                    var doNextCharsMatch = DoNextCharsMatchText(text, i, array);
                    if (doNextCharsMatch)
                    {
                        return i;
                    }
                }

            }
            const int notFoundIndicator = -1;
            return notFoundIndicator;
        }

        private static bool DoNextCharsMatchText(string text, int currentArrayIndex, byte[] array)
        {
            for (int j = 1; j < text.Length; j++)
            {
                var doesNextCharMatch = array[currentArrayIndex + j] == text[j];
                if (!doesNextCharMatch)
                {
                    return false;
                }
            }
            return true;
        }

        public static Dictionary<string, string> GetKeysAndValues(List<string> keys, byte[] array)
        {
            const int keyLength = 4;
            const int keysValueLength = 4;

            Dictionary<string, string> info = new Dictionary<string, string>();
            int currentIndex = 0;
            do
            {
                var nextFourChars = Converters.ConvertToString(array.SubArray(currentIndex, keyLength));
                var areNextFourCharsFoundInKeys = keys.Contains(nextFourChars);
                if (!areNextFourCharsFoundInKeys)
                {
                    break;

                }
                currentIndex += nextFourChars.Length;
                if (currentIndex + keysValueLength >= array.Length)
                {
                    break;
                }
                var sizeOfKeysValueBytes = array.SubArray(currentIndex, keysValueLength);
                var sizeOfKeysValue = (int)Converters.ConvertToUInt(sizeOfKeysValueBytes);

                currentIndex += keysValueLength;
                if (currentIndex + sizeOfKeysValue > array.Length)
                {
                    break;
                }
                var keysValue = Converters.ConvertToString(array.SubArray(currentIndex, sizeOfKeysValue));
                info[nextFourChars] = keysValue;

                currentIndex += sizeOfKeysValue;
                var tooCloseToEndOfArray = currentIndex > (array.Length - 1 - keyLength - keysValueLength);
                if (tooCloseToEndOfArray)
                {
                    break;
                }

            } while (true);
            return info;
        }
    }
}