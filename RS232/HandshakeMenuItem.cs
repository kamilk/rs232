using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialPortCommunicator.RS232.Parameters
{
    public class HandshakeMenuItem
    {
        public Handshake type { get; private set; }
        public HandshakeMenuItem(Handshake type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            switch (type)
            {
                case Handshake.RequestToSend:
                    return "RTS/CTR";
                case Handshake.XOnXOff:
                    return "CTRL-S/CTRL-Q";
                case Handshake.RequestToSendXOnXOff:
                    return "RTS/CTR CTRL-S/CTRL-Q";
                default:
                    return "Błędny typ";
            }
        }
    }
}
