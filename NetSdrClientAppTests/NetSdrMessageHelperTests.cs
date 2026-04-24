using NetSdrClientApp.Messages;

namespace NetSdrClientAppTests
{
    public class NetSdrMessageHelperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetControlItemMessageTest()
        {
            //Arrange
            var type = NetSdrMessageHelper.MsgTypes.Ack;
            var code = NetSdrMessageHelper.ControlItemCodes.ReceiverState;
            int parametersLength = 7500;

            //Act
            byte[] msg = NetSdrMessageHelper.GetControlItemMessage(type, code, new byte[parametersLength]);

            var headerBytes = msg.Take(2);
            var codeBytes = msg.Skip(2).Take(2);
            var parametersBytes = msg.Skip(4);

            var num = BitConverter.ToUInt16(headerBytes.ToArray());
            var actualType = (NetSdrMessageHelper.MsgTypes)(num >> 13);
            var actualLength = num - ((int)actualType << 13);
            var actualCode = BitConverter.ToInt16(codeBytes.ToArray());

            //Assert
            Assert.That(headerBytes.Count(), Is.EqualTo(2));
            Assert.That(msg.Length, Is.EqualTo(actualLength));
            Assert.That(type, Is.EqualTo(actualType));

            Assert.That(actualCode, Is.EqualTo((short)code));

            Assert.That(parametersBytes.Count(), Is.EqualTo(parametersLength));
        }

        [Test]
        public void GetDataItemMessageTest()
        {
            //Arrange
            var type = NetSdrMessageHelper.MsgTypes.DataItem2;
            int parametersLength = 7500;

            //Act
            byte[] msg = NetSdrMessageHelper.GetDataItemMessage(type, new byte[parametersLength]);

            var headerBytes = msg.Take(2);
            var parametersBytes = msg.Skip(2);

            var num = BitConverter.ToUInt16(headerBytes.ToArray());
            var actualType = (NetSdrMessageHelper.MsgTypes)(num >> 13);
            var actualLength = num - ((int)actualType << 13);

            //Assert
            Assert.That(headerBytes.Count(), Is.EqualTo(2));
            Assert.That(msg.Length, Is.EqualTo(actualLength));
            Assert.That(type, Is.EqualTo(actualType));

            Assert.That(parametersBytes.Count(), Is.EqualTo(parametersLength));
        }

        [Test]
        public void GetSamples_WithValid16BitData_ReturnsCorrectIntegers()
        {
            // Arrange
            ushort sampleSize = 16;
            byte[] body = new byte[] { 0x01, 0x00, 0x00, 0x01 };

            // Act
            var result = NetSdrMessageHelper.GetSamples(sampleSize, body).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(1));
            Assert.That(result[1], Is.EqualTo(256));
        }

        [Test]
        public void GetSamples_WithInvalidSampleSize_ThrowsException()
        {
            // Arrange
            ushort invalidSampleSize = 64; // > 4B (32 b)
            byte[] body = new byte[] { 0x01, 0x02 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                NetSdrMessageHelper.GetSamples(invalidSampleSize, body).ToList());
        }

        [Test]
        public void TranslateMessage_WithValidData_ReturnsTrueAndCorrectBody()
        {
            // Arrange
            var type = NetSdrMessageHelper.MsgTypes.Ack;
            var code = NetSdrMessageHelper.ControlItemCodes.RFFilter;
            byte[] dummyParams = new byte[] { 0x01, 0x02 }; 
            byte[] rawMessage = NetSdrMessageHelper.GetControlItemMessage(type, code, dummyParams);

            // Act
            bool success = NetSdrMessageHelper.TranslateMessage(
                rawMessage, out var outType, out var outCode, out var seqNum, out var outBody);

            // Assert
            Assert.That(success, Is.True);
            Assert.That(outType, Is.EqualTo(type));
            Assert.That(outCode, Is.EqualTo(code));
            Assert.That(outBody, Is.EquivalentTo(dummyParams));
        }
        
        [Test]
        public void TranslateMessage_WithInvalidControlItemCode_ReturnsFalse()
        {
            // Arrange
            var type = NetSdrMessageHelper.MsgTypes.Ack;
            var validCode = NetSdrMessageHelper.ControlItemCodes.ReceiverState;
            byte[] rawMessage = NetSdrMessageHelper.GetControlItemMessage(type, validCode, Array.Empty<byte>());

            rawMessage[2] = 0xFF;
            rawMessage[3] = 0xFF;

            // Act
            bool success = NetSdrMessageHelper.TranslateMessage(
                rawMessage, out var outType, out var outCode, out var seqNum, out var outBody);

            // Assert
            Assert.That(success, Is.False);
            Assert.That(outCode, Is.EqualTo(NetSdrMessageHelper.ControlItemCodes.None));
        }
    }
}