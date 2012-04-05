using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RS232.Transceivers
{
    interface ITransmitter
    {
        void TransmitData(SerialPort port, byte[] message);
        void SetXOFF();
        void SetXON();
    }
}
