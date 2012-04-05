using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerialPortCommunicator.Transceivers
{
    public class RTUTransceiver : ITransceiver
    {
        public ModbusMessage DataReceived(SerialPort port)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            while (port.BytesToRead > 0)
            {
                int bytes = port.BytesToRead;
                byte[] buffer = new byte[bytes];
                port.Read(buffer, 0, bytes);
                writer.Write(buffer);
            }

            byte[] response = memoryStream.ToArray();
            if (response.Length <= 4)
                throw new MessageException("Wiadomość za krótka");

            byte address = response[0];
            byte function = response[1];
            byte[] messageContent = new byte[response.Length - 4];
            Array.Copy(response, 2, messageContent, 0, messageContent.Length);
            ModbusMessage message = new ModbusMessage(address, function, messageContent);
            ValidateResponse(response, message);
            return message;
        }

        private void ValidateResponse(byte[] response, ModbusMessage message)
        {
            byte[] CRC = new byte[2];
            CRC[0] = response[response.Length - 2];
            CRC[1] = response[response.Length - 1];
            byte[] expectedCRC = CalculateCRC(message.Address, message.Function, message.Data);
            if (expectedCRC[0] != CRC[0] || expectedCRC[1] != CRC[1])
            {
                throw new MessageException("Błędna suma kontrolna");
            }
        }

        public void TransmitData(SerialPort port, ModbusMessage message)
        {
            //Clear in/out buffers:
            port.DiscardOutBuffer();
            port.DiscardInBuffer();

            byte[] RTUmessage = BuildMessage(message);
            port.Write(RTUmessage, 0, RTUmessage.Length);
        }

        private byte[] BuildMessage(ModbusMessage message)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(message.Address);
            writer.Write(message.Function);
            writer.Write(message.Data);
            byte[] CRC = CalculateCRC(message.Address, message.Function, message.Data);
            writer.Write(CRC);
            return memoryStream.ToArray();
        }

        private byte[] CalculateCRC(byte address, byte function, byte[] data)
        {
            byte[] CRC = new byte[2];
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;
            byte[] message = new byte[2 + data.Length];
            message[0] = address;
            message[1] = function;
            Array.Copy(data, 0, message, 2, data.Length);

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
            return CRC;
        }

    }

}
