using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}