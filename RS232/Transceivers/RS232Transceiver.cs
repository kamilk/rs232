using System.IO.Ports;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class RS232Transceiver : ITransceiver<RS232Message>
    {
        ITransmitter<RS232Message> Transmitter { get; set; }
        IReceiver<RS232Message> Receiver { get; set; }

        public RS232Transceiver(ITransmitter<RS232Message> transmitter, IReceiver<RS232Message> receiver)
        {
            Transmitter = transmitter;
            Receiver = receiver;
        }

        public RS232Message ReceiveMessage(SerialPort port)
        {
            return Receiver.ReceiveData(port);
        }

        public void TransmitMessage(SerialPort port, RS232Message message)
        {
            Transmitter.TransmitData(port, message);
        }
    }
}