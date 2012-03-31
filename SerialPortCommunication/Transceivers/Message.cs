using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SerialPortCommunicator.Transceivers
{
    public class Message
    {
        public byte Address { get; private set; }
        public byte Function { get; private set; }
        public byte[] Data { get; private set; }

        public Message(byte address, byte function, byte[] data)
        {
            Address = address;
            Function = function;
            Data = data;
        }
    }
}
