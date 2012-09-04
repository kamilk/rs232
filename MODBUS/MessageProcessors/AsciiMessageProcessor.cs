using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Transceivers;

namespace SerialPortCommunicator.Modbus.MessageProcessors
{
    class AsciiMessageProcessor : IModbusMessageProcessor
    {
        public void SendMessage(Rs232CommunicationManager rs232Manager, ModbusMessage message)
        {
            int messageLength = message.MessageHexString.Length;
            var data = new byte[messageLength + 7];

            data[0] = (byte)':';

            byte[] address = GetByteAsHex(message.Address);
            byte[] function = GetByteAsHex(message.Function);

            data[1] = address[0];
            data[2] = address[1];
            data[3] = function[0];
            data[4] = function[1];

            byte[] messageBytes = new ASCIIEncoding().GetBytes(message.MessageHexString);
            for (int i = 0; i < messageLength; i++)
                data[5 + i] = messageBytes[i];

            byte[] numericMessage = DecodeHexArray(data, 1, 4 + messageLength);
            byte[] lrc = GetByteAsHex(CalculateLRC(numericMessage));
            data[data.Length - 2] = lrc[0];
            data[data.Length - 1] = lrc[1];

            // The data is now 8 bits per character, but it's not a problem, it will
            // be reduced to 7 bits per character automatically.
            rs232Manager.SendMessage(new RS232Message(data));
        }

        public ModbusMessage ReceiveMessage(Rs232CommunicationManager rs232Manager)
        {
            return new ModbusMessage("", 0, 0);
        }

        private byte[] GetByteAsHex(byte value)
        {
            string valueString = string.Format(
                            "{0}{1:X}",
                            value < 16 ? "0" : "",
                            value);
            return new ASCIIEncoding().GetBytes(valueString);
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

        private byte[] DecodeHexArray(byte[] hexArray, int offset, int count)
        {
            var result = new byte[count / 2];
            var ascii = new ASCIIEncoding();

            for (int i = 0; i < count / 2; i++)
            {
                var hexBytes = new byte[] { hexArray[offset + i * 2], hexArray[offset + i * 2 + 1] };
                string hex = ascii.GetString(hexBytes);
                result[i] = Convert.ToByte(hex, 16);
            }

            return result;
        }
    }
}
