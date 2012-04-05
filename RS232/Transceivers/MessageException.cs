using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Transceivers
{
    public class MessageException : System.ApplicationException
    {
        public MessageException() { }
        public MessageException(string message) : base(message) { }
    }
}
