using System.IO.Ports;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics.Transceivers.Parameters;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class RS232Transceiver : ITransceiver<RS232Message>
    {
        ITransmitter<RS232Message> Transmitter { get; set; }
        IReceiver<RS232Message> Receiver { get; set; }

        public RS232Transceiver(ConnectionParameters parameters)
        {
            Transmitter = new Rs232Transmitter(parameters);
            Receiver = new Rs232Receiver(parameters);
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