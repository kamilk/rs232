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
        }

        public Message ReceiveMessage(SerialPort port)
        {
            try
            {
                return new Message(Receiver.ReceiveData(port));
            }
            catch (XOFFReceivedException)
            {
                Transmitter.SetXOFF();
                return null;
            }
            catch (XONReceivedException)
            {
                Transmitter.SetXON();
                return null;
            }
        }

        public void TransmitMessage(SerialPort port, Message message)
        {
            Transmitter.TransmitData(port, message.Data);
        }
    }
}
