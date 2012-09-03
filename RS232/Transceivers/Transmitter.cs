using System;
using System.Diagnostics;
using System.IO.Ports;
using SerialPortCommunicator.Generic.Parameters;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class Transmitter : ITransmitter<RS232Message>
    {
        public ConnectionParameters Parameters { get; set; }

        public Transmitter(ConnectionParameters parameters)
        {
            Parameters = parameters;
        }

        public void TransmitData(SerialPort port, RS232Message message)
        {
            byte[] data = message.BinaryData;

            Debug.WriteLine(data.Length);

            port.DiscardOutBuffer();

            int messageWriteOffset = 0;
            data = appendEndMarker(data);

            int bytesLeft = data.Length;

            while (bytesLeft > 0)
            {
                if (port.BytesToWrite < port.WriteBufferSize)
                {
                    if (port.WriteBufferSize - port.BytesToWrite < bytesLeft)
                    {
                        port.Write(data, messageWriteOffset, port.WriteBufferSize - port.BytesToWrite);
                        bytesLeft -= port.WriteBufferSize - port.BytesToWrite;
                    }
                    else
                    {
                        port.Write(data, messageWriteOffset, bytesLeft);
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
