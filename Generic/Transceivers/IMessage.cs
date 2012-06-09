using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using SerialPortCommunicator.Helpers;

namespace SerialPortCommunicator.Generics.Transceivers
{
    public interface IMessage
    {
        byte[] BinaryData { get; set; }
        string MessageString { get; set; }
    }
}
