using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SerialPortCommunicator.Properties;
using RS232.Parameters;
using System.Diagnostics;

namespace RS232.Transceivers
{
    public class Transmitter : ITransmitter
    {
        public ConnectionParameters Parameters { get; set; }

        public Transmitter(ConnectionParameters parameters)
        {
            Parameters = parameters;
        }

        public void TransmitData(SerialPort port, byte[] message)
        {
            Debug.WriteLine(message.Length);

            port.DiscardOutBuffer();

            int messageWriteOffset = 0;
            message = appendEndMarker(message);

            int bytesLeft = message.Length;

            while (bytesLeft > 0)
            {
                if (port.BytesToWrite < port.WriteBufferSize)
                {
                    if (port.WriteBufferSize - port.BytesToWrite < bytesLeft)
                    {
                        port.Write(message, messageWriteOffset, port.WriteBufferSize - port.BytesToWrite);
                        bytesLeft -= port.WriteBufferSize - port.BytesToWrite;
                    }
                    else
                    {
                        port.Write(message, messageWriteOffset, bytesLeft);
                        bytesLeft = 0;
                    }
                }
            }
        }

        private byte[] appendEndMarker(byte[] message)
        {
            byte[] ending = new byte[0];
            switch (Parameters.EndMarker)
            {
                case (EndMarker.CR):
                    ending = new byte[] { 13 };
                    break;
                case (EndMarker.CRLF):
                    ending = new byte[] { 13, 10 };
                    break;
                case (EndMarker.LF):
                    ending = new byte[] { 10 };
                    break;
                default:
                    break;
            }
            byte[] result = new byte[message.Length + ending.Length];
            Array.Copy(message, result, message.Length);
            Array.Copy(ending, 0, result, message.Length, ending.Length);
            return result;
        }
    }
}
