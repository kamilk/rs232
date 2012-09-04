using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SerialPortCommunicator.Generics.Helpers;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class RS232Message
    {
        public byte[] BinaryData { get; set; }
        public string MessageString { get; set; }

        public RS232Message(byte[] data)
        {
            BinaryData = data;
            MessageString = data.DeserializedString().Replace("\n", "<LF>").Replace("\r", "<CR>");
        }

        public RS232Message(string messageString)
        {
            BinaryData = messageString.SerializedString();
            MessageString = messageString;
        }
    }
}
