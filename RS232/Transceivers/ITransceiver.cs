using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace RS232.Transceivers
{
    public interface ITransceiver
    {
        Message ReceiveMessage(SerialPort port);
        void TransmitMessage(SerialPort port, Message message);
    }
}
