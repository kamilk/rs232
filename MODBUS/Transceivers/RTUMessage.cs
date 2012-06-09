using System;
using System.IO;

using SerialPortCommunicator.Helpers;

namespace SerialPortCommunicator.MODBUS.Transceivers
{
    public class RTUMessage : ModbusMessage
    {
        public RTUMessage(string messageString, byte address, byte function)
            : base(messageString, address, function)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(Address);
            writer.Write(Function);
            writer.Write(messageString.SerializedString());
            byte[] CRC = CalculateCRC(messageString.SerializedString());
            writer.Write(CRC);

            BinaryData = memoryStream.ToArray();
        }

        private byte[] CalculateCRC(byte[] data)
        {
            byte[] CRC = new byte[2];
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;
            byte[] message = new byte[2 + data.Length];
            message[0] = Address;
            message[1] = Function;
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
