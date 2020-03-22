using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveFileManipulator;

namespace WaveFileManipulatorTests
{
    [TestClass]
    public class SamplesManipulatorTests
    {
        [TestMethod]
        public void ByteArraySamplesAreReversed()
        {
            //Arrange
            const int bytesPerSample = 2;
            byte[] forwardsArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            byte[] expectedReversedArray = { 8, 9, 6, 7, 4, 5, 2, 3, 0, 1 };

            //Act
            var reversedArray = SamplesManipulator.Reverse(bytesPerSample, forwardsArray);

            //Assert
            Assert.IsTrue(reversedArray.SequenceEqual(expectedReversedArray));
        }

        [TestMethod]
        public void ByteCollectionSamplesAreReversed()
        {
            //Arrange
            const int bytesPerSample = 2;
            var forwardsList = new List<byte> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            byte[] expectedReversedArray = { 8, 9, 6, 7, 4, 5, 2, 3, 0, 1 };

            //Act
            var reversedArray = SamplesManipulator.Reverse(bytesPerSample, forwardsList);

            //Assert
            Assert.IsTrue(reversedArray.SequenceEqual(expectedReversedArray));
        }
    }
}