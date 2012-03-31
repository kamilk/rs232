using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace SerialPortCommunicator.Properties
{
    class ConnectionParameters
    {
        public string PortName { get; private set; }
        public Parity Parity { get; private set; }
        public Handshake Handshake { get; private set; }
        public StopBits StopBits { get; private set; }
        public int BaudRate { get; private set; }
        public int DataBits { get; set; }

        public ConnectionParameters(string portName, int baudRate, int dataBitsCount, Parity parity, Handshake handshakeType, StopBits stopBitsType)
        {
            PortName = portName;
            BaudRate = baudRate;
            DataBits = dataBitsCount;
            Parity = parity;
            Handshake = handshakeType;
            StopBits = stopBitsType;
        }
    }
}
