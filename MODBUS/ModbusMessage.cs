using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics.Helpers;

namespace SerialPortCommunicator.Modbus
{
    public class ModbusMessage
    {
        public string MessageHexString { get; set; }
        public byte Address { get; set; }
        public byte Function { get; set; }

        public ModbusMessage(string messageString, byte address, byte function)
        {
            MessageHexString = messageString.Replace(" ", "");
            Address = address;
            Function = function;
        }
    }
}
