using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics.Helpers;
using SerialPortCommunicator.Generic.Helpers;

namespace SerialPortCommunicator.Modbus
{
    public class ModbusMessage
    {
        public byte[] Data { get; set; }
        public byte Address { get; set; }
        public byte Function { get; set; }

        public ModbusMessage(string messageHexString, byte address, byte function)
        {
            Data = HexHelper.DecodeHexString(messageHexString);
            Address = address;
            Function = function;
        }

        public ModbusMessage(byte[] messageData, byte address, byte function)
        {
            Data = messageData;
            Address = address;
            Function = function;
        }
    }
}
