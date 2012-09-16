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

        public bool IsBroadcast
        { get { return Address == 0; } }

        public ModbusMessage(byte[] messageData, byte address, byte function)
        {
            Data = messageData;
            Address = address;
            Function = function;
        }

        public ModbusMessage(string messageHexString, byte address, byte function)
            : this(HexHelper.DecodeHexString(messageHexString), address, function)
        { }

        public ModbusMessage(byte address, byte function)
            : this(new byte[0], address, function)
        { }
    }
}
