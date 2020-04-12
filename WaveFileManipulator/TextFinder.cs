using System;
using System.Collections.Generic;
using System.Text;

namespace WaveFileManipulator
{
    public static class TextFinder
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
    }
}