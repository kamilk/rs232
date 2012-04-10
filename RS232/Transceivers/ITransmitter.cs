using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RS232.Transceivers
{
    public interface ITransmitter
    {
        void TransmitData(SerialPort port, byte[] message);
    }
}
