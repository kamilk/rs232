using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics;

namespace SerialPortCommunicator.Generics
{
    public class LogEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public MessageType MessageType { get; private set; }

        public LogEventArgs(string message, MessageType type = MessageType.Normal)
        {
            Message = message;
        }
    }
}
