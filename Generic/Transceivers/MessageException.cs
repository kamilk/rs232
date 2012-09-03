using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Generics.Transceivers
{
    public class MessageException : ApplicationException
    {
        public MessageException() { }
        public MessageException(string message) : base(message) { }
    }
}
