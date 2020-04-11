using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveFileManipulator;

namespace WaveFileManipulatorTests
{
    [TestClass]
    public class TextFinderTests
    {
        [TestMethod]
        public void TextIsFound()
        {
            //Arrange
            byte[] array = { 4, 21, 45, 100, 97, 116, 97, 21, 72, 34 };
            var text = "data";

            //Act
            var startIndex = TextFinder.GetStartIndexOfText(array, text);

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
            var startIndex = TextFinder.GetStartIndexOfText(array, text);

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
            var startIndex = TextFinder.GetStartIndexOfText(array, text);

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
            var startIndex = TextFinder.GetStartIndexOfText(array, text);

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
            var startIndex = TextFinder.GetStartIndexOfText(array, text);

            //Assert
            Assert.AreEqual(-1, startIndex);
        }
    }
}