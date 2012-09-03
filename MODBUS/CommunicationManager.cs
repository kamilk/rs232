
using SerialPortCommunicator.Modbus.Transceivers;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.GUI;
using SerialPortCommunicator.Generics.Transceivers;

namespace SerialPortCommunicator.Modbus.Communicator
{
    public class CommunicationManager : SerialPortCommunicator.Generic.Communicator.CommunicationManager<RTUMessage>
    {
        public CommunicationManager(ConnectionParameters connectionParameters, ProgramWindow programWindow, ITransceiver<RTUMessage> transceiver)
            : base(connectionParameters, programWindow, transceiver)
        {
        }
        public override void WriteData(string msg)
        {
            Transceiver.TransmitMessage(comPort, new RTUMessage(msg, 0, 0));
        }
    }
}
