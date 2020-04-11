using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using WaveFileManipulator;

namespace WaveFileManipulatorTests
{
    [TestClass]
    public class MetadataTests
    {
        byte[] dataArray =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                40, 0, 0, 0, //ChunkSize = 40
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 0, 0, 0,  //SubChunk2Size = 4
                1, 2, 3, 4}; //2 samples of fake data

        byte[] listArray =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                70, 96, 115, 2, //ChunkSize = 41115718
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                76, 73, 83, 84, //SubChunk2Id = "LIST"
                26, 0, 0, 0, //SubChunk2Size = 26
                73, 78, 70, 79, 73, 83, 70, 84, 14, 0, 0, 0, 76, 97, 118, 102, 53, 55, 46, 55, 54, 46, 49, 48, 48, 0, //26 bytes of data
                100, 97, 116, 97, //"data"
                4, 0, 0, 0, //num of data bytes
                1, 2, 3, 4}; //2 samples of fake data

        [TestMethod]
        public void CorrectArrayDoesNotThrowException()
        {
            //Arrange
            byte[] array =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                40, 0, 0, 0, //ChunkSize = 40 = Correct
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 0, 0, 0,  //SubChunk2Size = 4
                1, 2, 3, 4}; //2 samples of fake data

            //Act
            _ = new Metadata(array);
        }
        
        [TestMethod]
        public void IncorrectChunkSize()
        {
            //Arrange
            byte[] array =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                44, 0, 0, 0, //ChunkSize = 44 = Incorrect
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 0, 0, 0,  //SubChunk2Size = 4
                1, 2, 3, 4}; //2 samples of fake data      

            //Act
            var metadata = new Metadata(array);

            //Assert
            Assert.AreEqual(false, metadata.ChunkSize.IsValueExpected);
        }        

        [TestMethod]
        public void IncorrectByteRate()
        {
            //Arrange
            byte[] array =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                40, 0, 0, 0, //ChunkSize = 40
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 1, //ByteRate = Incorrect
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 0, 0, 0,  //SubChunk2Size = 4
                1, 2, 3, 4}; //2 samples of fake data      

            //Act
            var metadata = new Metadata(array);

            //Assert
            Assert.AreEqual(false, metadata.ByteRate.IsValueExpected);
        }

        [TestMethod]
        public void IncorrectBlock()
        {
            //Arrange
            byte[] array =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                40, 0, 0, 0, //ChunkSize = 40
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 3, //BlockAlign = Incorrect
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 0, 0, 0,  //SubChunk2Size = 4
                1, 2, 3, 4}; //2 samples of fake data      

            //Act
            var metadata = new Metadata(array);

            //Assert
            Assert.AreEqual(false, metadata.BlockAlign.IsValueExpected);
        }

        [TestMethod]
        public void IncorrectSubChunk2SizeThrowsException()
        {
            //Arrange
            byte[] array =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                40, 0, 0, 0, //ChunkSize = 40
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 21, 0, 0,  //SubChunk2Size = Incorrect
                1, 2, 3, 4}; //2 samples of fake data      

            //Act
            var metadata = new Metadata(array);

            //Assert
            Assert.AreEqual(false, metadata.SubChunk2Size.IsValueExpected);
        }

        //TODO: Test this with multiple sub-chunks
        [TestMethod]
        public void DataStartIndexIsCorrect()
        {
            //Arrange
            byte[] array =
                { 82, 73, 70, 70, //ChunkId = "RIFF"
                40, 0, 0, 0, //ChunkSize = 40
                87, 65, 86, 69, //Format = "WAVE"
                102, 109, 116, 32, //SubChunk1Id = "fmt "
                16, 0, 0, 0, //SubChunk1Size = 16
                1, 0, //AudioFormat = 1
                2, 0, //NumOfChannels = 2
                68, 172, 0, 0, //SampleRate = 44100
                16, 177, 2, 0, //ByteRate = 176400
                4, 0, //BlockAlign = 4
                16, 0, //BitsPerSample = 16
                100, 97, 116, 97, //SubChunk2Id = "data"
                4, 0, 0, 0,  //SubChunk2Size = 4
                1, 2, 3, 4}; //2 samples of fake data

            //Act
            var metadata = new Metadata(array);

            //Assert
            Assert.AreEqual(44, metadata.DataStartIndex);
        }
    }
}