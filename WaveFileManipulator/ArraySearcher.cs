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

        //TODO: test this
        public static Dictionary<string, string> GetKeysAndValues(List<string> keys, byte[] array)
        {
            const int infoIdLength = 4;
            const int infoIdSizeLength = 4;

            Dictionary<string, string> info = new Dictionary<string, string>();
            bool shouldContinueSearching;
            int currentIndex = 0;
            do
            {
                var nextFourChars = Converters.ConvertToString(array.SubArray(currentIndex, infoIdLength));
                var areNextFourCharsInfoTextId = keys.Contains(nextFourChars);
                if (areNextFourCharsInfoTextId)
                {
                    currentIndex = currentIndex + nextFourChars.Length;
                    var sizeOfIntoTextArray = array.SubArray(currentIndex, infoIdSizeLength);
                    var sizeOfInfoText = (int)Converters.ConvertToUInt(sizeOfIntoTextArray);
                    currentIndex = currentIndex + infoIdSizeLength;
                    var infoText = Converters.ConvertToString(array.SubArray(currentIndex, sizeOfInfoText));
                    info[nextFourChars] = infoText;
                    currentIndex = currentIndex + sizeOfInfoText;
                    shouldContinueSearching = true;
                }
                else
                {
                    shouldContinueSearching = false;
                }
            } while (shouldContinueSearching);
            return info;
        }
    }
}