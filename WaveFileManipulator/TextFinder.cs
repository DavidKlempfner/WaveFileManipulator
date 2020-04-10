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
                    for (int j = 1; j < text.Length; j++)
                    {
                        var doesNextCharMatch = array[i + j] == text[j];
                        if (!doesNextCharMatch)
                        {
                            break;
                        }

                        bool isEndOfLoop = j == text.Length - 1;
                        if (isEndOfLoop)
                        {
                            var startIndex = i + j - text.Length + 1;
                            return startIndex;
                        }
                    }
                }

            }
            throw new ArgumentException($"{text} was not found.", nameof(array));
        }
    }
}