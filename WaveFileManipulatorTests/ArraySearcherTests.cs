using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WaveFileManipulator;

namespace WaveFileManipulatorTests
{
    [TestClass]
    public class ArraySearcherTests
    {
        [TestMethod]
        public void TextIsFound()
        {
            //Arrange
            byte[] array = { 4, 21, 45, 100, 97, 116, 97, 21, 72, 34 };
            var text = "data";

            //Act
            var startIndex = ArraySearcher.GetStartIndexOfText(array, text);

            //Assert
            Assert.AreEqual(3, startIndex);
        }

        [TestMethod]
        public void TextAtStartIsFound()
        {
            //Arrange
            byte[] array = { 100, 97, 116, 97, 21, 72, 34 };
            var text = "data";

            //Act
            var startIndex = ArraySearcher.GetStartIndexOfText(array, text);

            //Assert
            Assert.AreEqual(0, startIndex);
        }

        [TestMethod]
        public void TextAtEndIsFound()
        {
            //Arrange
            byte[] array = { 21, 72, 3, 100, 97, 116, 97 };
            var text = "data";

            //Act
            var startIndex = ArraySearcher.GetStartIndexOfText(array, text);

            //Assert
            Assert.AreEqual(3, startIndex);
        }

        [TestMethod]
        public void NoSpaceLeftToContinueSearching()
        {
            //Arrange
            byte[] array = { 21, 72, 3, 100, 97, 116 };
            var text = "data";

            //Act
            var startIndex = ArraySearcher.GetStartIndexOfText(array, text);

            //Assert
            Assert.AreEqual(-1, startIndex);
        }

        [TestMethod]
        public void TextNotFound()
        {
            //Arrange
            byte[] array = { 21, 72, 3, 104, 98, 136 };
            var text = "data";

            //Act
            var startIndex = ArraySearcher.GetStartIndexOfText(array, text);

            //Assert
            Assert.AreEqual(-1, startIndex);
        }

        [TestMethod]
        public void KeyAndValueFound()
        {
            //Arrange
            byte[] array =
            {
                73, 83, 70, 84, //"ISFT"
                14, 0, 0, 0, //size
                76, 97, 118, 102, 53, 55, 46, 55, 54, 46, 49, 48, 48, 0,//"Lavf57.76.100\0"
                100, 97, 116, 97, 0, 96, 115, 2, //random bytes
            };
            var keys = new List<string> {  "ISFT" };

            //Act
            var keysAndValues = ArraySearcher.GetKeysAndValues(keys, array);

            //Assert
            Assert.AreEqual(1, keysAndValues.Keys.Count);
            Assert.IsTrue(keysAndValues.ContainsKey("ISFT"));
            Assert.AreEqual("Lavf57.76.100\0", keysAndValues["ISFT"]);
        }

        [TestMethod]
        public void KeysAndValuesFound()
        {
            //Arrange
            byte[] array =
            {
                73, 65, 82, 76, //"IARL"
                10, 0, 0, 0, //size
                76, 97, 118, 102, 53, 55, 46, 55, 54, 46,//"Lavf57.76."
                73, 65, 82, 84, //"IART"
                8, 0, 0, 0, //size
                76, 97, 118, 102, 76, 97, 118, 102,//"LavfLavf"
                0, 96, 115, 2 //random bytes
            };
            var keys = new List<string> { "IARL", "IART", "ICMS" };

            //Act
            var keysAndValues = ArraySearcher.GetKeysAndValues(keys, array);

            //Assert
            Assert.AreEqual(2, keysAndValues.Keys.Count);
            Assert.IsTrue(keysAndValues.ContainsKey("IARL"));
            Assert.AreEqual("Lavf57.76.", keysAndValues["IARL"]);
            Assert.IsTrue(keysAndValues.ContainsKey("IART"));
            Assert.AreEqual("LavfLavf", keysAndValues["IART"]);
        }

        [TestMethod]
        public void KeyAndValueFoundAtEndOfArray()
        {
            //Arrange
            byte[] array =
            {
                73, 65, 82, 76, //"IARL"
                1, 0, 0, 0, //size
                76//"L"
            };
            var keys = new List<string> { "IARL", "IART", "ICMS" };

            //Act
            var keysAndValues = ArraySearcher.GetKeysAndValues(keys, array);

            //Assert
            Assert.AreEqual(1, keysAndValues.Keys.Count);
            Assert.IsTrue(keysAndValues.ContainsKey("IARL"));
            Assert.AreEqual("L", keysAndValues["IARL"]);
        }

        [TestMethod]
        public void KeyTooCloseToEndNotFound()
        {
            //Arrange
            byte[] array =
            {
                73, 65, 82, 76, //"IARL"
                1, 0, 0, 0 //size
            };
            var keys = new List<string> { "IARL", "IART", "ICMS" };

            //Act
            var keysAndValues = ArraySearcher.GetKeysAndValues(keys, array);

            //Assert
            Assert.AreEqual(0, keysAndValues.Keys.Count);
        }
    }
}