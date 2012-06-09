using System.IO.Ports;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class RS232Transceiver : ITransceiver<RS232Message>
    {
        ITransmitter Transmitter { get; set; }
        IReceiver Receiver { get; set; }

        public RS232Transceiver(ITransmitter transmitter, IReceiver receiver)
        {
            Transmitter = transmitter;
            Receiver = receiver;
        }

        public RS232Message ReceiveMessage(SerialPort port)
        {
            return new RS232Message(Receiver.ReceiveData(port));
        }

        public void TransmitMessage(SerialPort port, RS232Message message)
        {
            Transmitter.TransmitData(port, message.BinaryData);
        }
    }
}