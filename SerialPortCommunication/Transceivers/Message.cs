using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RS232.Transceivers
{
    public class Message
    {
        public byte[] Data { get; protected set; }

        public Message(byte[] data)
        {
            Data = data;
        }
    }

    public class ModbusMessage : Message
    {
        public byte Address { get; protected set; }
        public byte Function { get; protected set; }

        public ModbusMessage(byte address, byte function, byte[] data)
            : base(data)
        {
            Address = address;
            Function = function;
        }
    }
}
