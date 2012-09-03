using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Helpers;

namespace SerialPortCommunicator.Modbus.Transceivers
{
    public abstract class ModbusMessage: IMessage
    {
        public byte[] BinaryData { get; set; }
        public string MessageString { get; set; }
        public byte Address { get; protected set; }
        public byte Function { get; protected set; }

        public ModbusMessage(string messageString, byte address, byte function)
        {
            BinaryData = messageString.SerializedString();
            MessageString = messageString;

            Address = address;
            Function = function;
        }
    }
}
