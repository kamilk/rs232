using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerialPortCommunicator.Transceivers
{
    public class AsciiTransceiver : ITransceiver
    {
        byte[] ASCIIChars = { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70 };
        
        public ModbusMessage DataReceived(SerialPort port)
        {
            int bytes = port.BytesToRead;
            byte[] response = new byte[bytes];
            port.Read(response, 0, bytes);
            if (bytes <= 9)
                throw new MessageException("Wiadomość za krótka");

            byte address = HexToByte(response, 1);
            byte function = HexToByte(response, 3);
            int messageLength = (response.Length - 1 - 2 - 2 - 2 - 2) / 2;
            byte[] messageContent = new byte[messageLength];
            for (int i = 0; i <  messageLength; i++)
			{
			     messageContent[i] = HexToByte(response, 5 + i*2);
			}
            ModbusMessage message = new ModbusMessage(address, function, messageContent);
            ValidateResponse(response, message);
            return message;
        }

        private void ValidateResponse(byte[] response, ModbusMessage message)
        {
            if (response[0] != (byte)':')
            {
                throw new MessageException("Błędny znacznik początku");
            }
            byte LRC = HexToByte(response, response.Length - 4);
            if (LRC != CalculateLRC(message.Address, message.Function, message.Data))
            {
                throw new MessageException("Błędna suma kontrolna");
            }
            if (response[response.Length - 2] != (byte)'\r' || response[response.Length - 1] != (byte)'\n')
            {
                throw new MessageException("Błędny znacznik końca");
            }
        }

        public void TransmitData(SerialPort port, ModbusMessage message)
        {
            //Clear in/out buffers:
            port.DiscardOutBuffer();
            port.DiscardInBuffer();
            
            byte[] ASCIImessage = BuildMessage(message);
            port.Write(ASCIImessage, 0, ASCIImessage.Length);
        }

        private byte[] BuildMessage(ModbusMessage message)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write((byte)':');
            writer.Write(ByteToHex(message.Address));
            writer.Write(ByteToHex(message.Function));
            foreach (var b in message.Data)
            {
                writer.Write(ByteToHex(b));
            }
            byte LRC = CalculateLRC(message.Address, message.Function, message.Data);
            writer.Write(ByteToHex(LRC));
            writer.Write((byte)'\r');
            writer.Write((byte)'\n');
            return memoryStream.ToArray();
        }

        private byte CalculateLRC(byte address, byte function, byte[] data)
        {
            byte LRC = (byte)0;
            LRC += address;
            LRC += function;
            for (int i = 0; i < data.Length; i++)
            {
                LRC += data[i]; // (x & 0xFF) is implicit when storing to a byte.
            }
            LRC = (byte)((LRC ^ 0xFF) + 1); // (x & 0xFF) is implicit when storing to a byte.
            return LRC;
        }

        private byte[] ByteToHex(byte b)
        {
            byte[] result = new byte[2];
            result[0] = ASCIIChars[b >> 4];
            result[1] = ASCIIChars[b & 0x0F];
            return result;
        }

        private byte HexToByte(byte[] h, int offset)
        {
            return (byte)(Array.BinarySearch(ASCIIChars, h[offset]) * 16 + Array.BinarySearch(ASCIIChars, h[offset+1]));
        }

    }
}
