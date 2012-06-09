using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.Generics.Transceivers
{
    public interface ITransceiver<TMessage> where TMessage : IMessage
    {
        TMessage ReceiveMessage(SerialPort port);
        void TransmitMessage(SerialPort port, TMessage message);
    }
}
