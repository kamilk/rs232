using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace SerialPortCommunicator.Transceivers
{
    public interface ITransceiver
    {
        Message DataReceived(SerialPort port);
        void TransmitData(SerialPort port, Message message);
    }
}
