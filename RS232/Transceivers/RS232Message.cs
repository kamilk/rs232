﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using SerialPortCommunicator.Helpers;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class RS232Message : IMessage
    {
        public byte[] BinaryData { get; set; }
        public string MessageString { get; set; }

        public RS232Message(byte[] data)
        {
            BinaryData = data;
            MessageString = data.DeserializedString();
        }

        public RS232Message(string messageString)
        {
            BinaryData = messageString.SerializedString();
            MessageString = messageString;
        }
    }
}
