using System;
using System.IO.Ports;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.MODBUS.Transceivers
{
    public class RTUTransceiver : ITransceiver<RTUMessage>
    {
        ITransmitter Transmitter { get; set; }
        IReceiver Receiver { get; set; }

        public RTUTransceiver(ITransmitter transmitter, IReceiver receiver)
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
            Transmitter.TransmitData(port, message.BinaryData);
        }
    }
}