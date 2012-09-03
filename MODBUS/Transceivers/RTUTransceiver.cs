using System;
using System.IO.Ports;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.Modbus.Transceivers
{
    public class RTUTransceiver : ITransceiver<RTUMessage>
    {
        ITransmitter<RTUMessage> Transmitter { get; set; }
        IReceiver<RTUMessage> Receiver { get; set; }

        public RTUTransceiver(ITransmitter<RTUMessage> transmitter, IReceiver<RTUMessage> receiver)
        {
            Transmitter = transmitter;
            Receiver = receiver;
        }

        public RTUMessage ReceiveMessage(SerialPort port)
        {
            throw new NotImplementedException();
        }

        public void TransmitMessage(SerialPort port, RTUMessage message)
        {
            Transmitter.TransmitData(port, message);
        }
    }
}