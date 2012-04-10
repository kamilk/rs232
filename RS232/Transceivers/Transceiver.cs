using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RS232.Transceivers
{
    public class Transceiver : ITransceiver
    {
        ITransmitter Transmitter { get; set; }
        IReceiver Receiver { get; set; }

        public Transceiver(ITransmitter transmitter, IReceiver receiver)
        {
            Transmitter = transmitter;
            Receiver = receiver;
        }

        public Message ReceiveMessage(SerialPort port)
        {
            return new Message(Receiver.ReceiveData(port));
        }

        public void TransmitMessage(SerialPort port, Message message)
        {
            Transmitter.TransmitData(port, message.Data);
        }
    }
}
