using System.Text;
using SerialPortCommunicator.Generic.Helpers;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Transceivers;

namespace SerialPortCommunicator.Modbus.MessageProcessors
{
    class AsciiMessageProcessor : IModbusMessageProcessor
    {
        /// <summary>
        /// start + address + command + LRC + 1 byte of data as 2-byte hex
        /// </summary>
        private const int MINIMUM_MESSAGE_LENGTH = 9;

        public void SendMessage(Rs232CommunicationManager rs232Manager, ModbusMessage message)
        {
            int messageLength = message.MessageHexString.Length;
            var data = new byte[messageLength + 7];

            data[0] = (byte)':';

            byte[] address = HexHelper.GetByteAsHex(message.Address);
            byte[] function = HexHelper.GetByteAsHex(message.Function);

            data[1] = address[0];
            data[2] = address[1];
            data[3] = function[0];
            data[4] = function[1];

            byte[] messageBytes = new ASCIIEncoding().GetBytes(message.MessageHexString);
            for (int i = 0; i < messageLength; i++)
                data[5 + i] = messageBytes[i];

            byte[] numericMessage = HexHelper.DecodeHexArray(data, 1, 4 + messageLength);
            byte[] lrc = HexHelper.GetByteAsHex(CalculateLRC(numericMessage));
            data[data.Length - 2] = lrc[0];
            data[data.Length - 1] = lrc[1];

            // The data is now 8 bits per character, but it's not a problem, it will
            // be reduced to 7 bits per character automatically.
            rs232Manager.SendMessage(new RS232Message(data));
        }

        public ModbusMessage ProcessMessage(RS232Message message)
        {
            byte[] data = message.BinaryData;

            int messageStart;
            for (messageStart = 0; messageStart < data.Length; messageStart++)
                if (data[messageStart] == (byte)':')
                    break;

            int messageLength = data.Length - messageStart;
            if (messageLength < MINIMUM_MESSAGE_LENGTH)
                return null;

            int lrc = HexHelper.DecodeHex(data[data.Length - 2], data[data.Length - 1]);
            int expectedLrc = CalculateLRC(HexHelper.DecodeHexArray(data, messageStart + 1, data.Length - messageStart - 3));
            if (lrc != expectedLrc)
                return null;

            byte address = HexHelper.DecodeHex(data[messageStart + 1], data[messageStart + 2]);
            byte function = HexHelper.DecodeHex(data[messageStart + 3], data[messageStart + 4]);
            byte[] messageContent = HexHelper.DecodeHexArray(data, messageStart + 5, messageLength - 7);

            return new ModbusMessage(messageContent, address, function);
        }

        /// <summary>
        /// From http://en.wikipedia.org/wiki/Longitudinal_redundancy_check
        /// 
        /// Longitudinal Redundancy Check (LRC) calculator for a byte array. 
        /// This was proved from the LRC Logic of Edwards TurboPump Controller SCU-1600.
        /// ex) DATA (hex 6 bytes): 02 30 30 31 23 03
        ///     LRC  (hex 1 byte ): EC        
        /// </summary>
        public byte CalculateLRC(byte[] bytes)
        {
            byte LRC = 0x00;
            for (int i = 0; i < bytes.Length; i++)
            {
                LRC = (byte)((LRC + bytes[i]) & 0xFF);
            }
            return (byte)(((LRC ^ 0xFF) + 1) & 0xFF);
        }
    }
}
