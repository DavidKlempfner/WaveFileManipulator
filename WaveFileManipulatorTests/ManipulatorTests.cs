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
        private byte[] PopulateForwardsWavFileByteArray(string forwardsWavFilePath)
        {
            byte[] forwardsWavFileStreamByteArray;
            using (FileStream forwardsWavFileStream = new FileStream(forwardsWavFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                forwardsWavFileStreamByteArray = new byte[forwardsWavFileStream.Length];
                forwardsWavFileStream.Read(forwardsWavFileStreamByteArray, 0, (int)forwardsWavFileStream.Length);
            }

            return forwardsWavFileStreamByteArray;
        }

        [TestMethod]
        public void Run()
        {
            var manipulator = new Manipulator();
            var reversedByteArray = manipulator.Reverse(@"C:\Users\David'\Desktop\WavFiles\out.wav");
            WriteReversedWavFileByteArrayToFile(reversedByteArray, @"C:\Users\David'\Desktop\WavFiles\16BitPCM\ReversedShort.wav");
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NonWaveFileContentFormatThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();

            //Act
            manipulator.Reverse(@"C:\Users\David'\Desktop\WavFiles\16BitPCM\notWavFormat.wav");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooSmallFileThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();

            //Act
            manipulator.Reverse(@"C:\Users\David'\Desktop\WavFiles\16BitPCM\tooSmall.wav");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArrayThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();
            IEnumerable<byte> array = null;

            //Act
            manipulator.Reverse(array);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullStringThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();
            string array = null;

            //Act
            manipulator.Reverse(array);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyStringThrowsException()
        {
            //Arrange
            var manipulator = new Manipulator();            

            //Act
            manipulator.Reverse("");
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