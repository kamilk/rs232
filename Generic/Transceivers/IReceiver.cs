using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialPortCommunicator.Generics.Transceivers
{
    public interface IReceiver<T>
    {
        IEnumerable<T> ReceiveMessages(SerialPort port);
    }
}
