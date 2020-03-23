using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using WaveFileManipulator;

namespace WaveFileManipulatorTests
{
    [TestClass]
    public class ManipulatorTests
    {        
        [TestMethod]
        public void ReversedFileIsSameSizeAsOriginal()
        {
            //Arrange
            var manipulator = new Manipulator();
            var expectedByteArray = new byte[35992];

            //Act
            var reversedByteArray = manipulator.Reverse(@"C:\Users\David'\Desktop\WavFiles\16BitPCM\Short.wav");

            //Assert
            Assert.AreEqual(expectedByteArray.Length, reversedByteArray.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void NonExistentFileThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();

            //Act
            manipulator.Reverse(@"C:\thisFileDoesntExist.wav");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongFileExtensionThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();

            //Act
            manipulator.Reverse(@"C:\thisFileDoesntExist.txt");
        }
    }
}