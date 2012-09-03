using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Generic
{
    public class DataReceivedEventArgs<TMessage> : EventArgs
    {
        public DataReceivedEventArgs(TMessage receivedMessage)
        {
            Message = receivedMessage;
        }
        public TMessage Message { get; private set; }
    }
}
