using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using WaveFileManipulator;

namespace WaveFileManipulatorTests
{
    [TestClass]
    public class ManipulatorTests
    {                
        [TestMethod]
        public void Run()
        {
            //var filePath = @"C:\Users\David'\Desktop\WavFiles\out.wav";
            var filePath = @"C:\Users\David'\Desktop\WavFiles\16BitPCM\Backwards.wav";
            var manipulator = new Manipulator(filePath);
            var reversedByteArray = manipulator.Reverse();
            
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var fileDirectoryPath = Path.GetDirectoryName(filePath);
            var outputFilePath = Path.Combine(fileDirectoryPath, fileNameWithoutExtension + "Reversed.wav");
            WriteReversedWavFileByteArrayToFile(reversedByteArray, outputFilePath);
        }

        [TestMethod]
        public void ReversedFileIsSameSizeAsOriginal()
        {
            //Arrange
            var manipulator = new Manipulator(@"C:\Users\David'\Desktop\WavFiles\16BitPCM\Short.wav");
            var expectedByteArray = new byte[35992];

            //Act
            var reversedByteArray = manipulator.Reverse();            
            
            //Assert
            Assert.AreEqual(expectedByteArray.Length, reversedByteArray.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void NonExistentFileThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator(@"C:\thisFileDoesntExist.wav");

            //Act
            manipulator.Reverse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongFileExtensionThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator(@"C:\thisFileDoesntExist.txt");

            //Act
            manipulator.Reverse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NonWaveFileContentFormatThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator(@"C:\Users\David'\Desktop\WavFiles\16BitPCM\notWavFormat.wav");

            //Act
            manipulator.Reverse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooSmallFileThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator(@"C:\Users\David'\Desktop\WavFiles\16BitPCM\tooSmall.wav");

            //Act
            manipulator.Reverse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArrayThrowsException()
        {
            //Arrange
            IEnumerable<byte> array = null;
            var manipulator = new Manipulator(array);            

            //Act
            manipulator.Reverse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullStringThrowsException()
        {
            //Arrange
            string array = null;
            var manipulator = new Manipulator(array);
            
            //Act
            manipulator.Reverse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyStringThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator("");            

            //Act
            manipulator.Reverse();
        }        

        private static void WriteReversedWavFileByteArrayToFile(byte[] reversedWavFileStreamByteArray, string reversedWavFilePath)
        {
            using (FileStream reversedFileStream = new FileStream(reversedWavFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                reversedFileStream.Write(reversedWavFileStreamByteArray, 0, reversedWavFileStreamByteArray.Length);
            }
        }
    }
}