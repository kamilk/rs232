using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialPortCommunicator.Generics.Transceivers
{
    public interface ITransmitter<T>
    {
        void TransmitData(SerialPort port, T message);
    }
}
