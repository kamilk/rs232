using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics.Transceivers;
using System.IO.Ports;
using SerialPortCommunicator.RS232.Communicator;

namespace SerialPortCommunicator.Modbus.Transceivers
{
    abstract class ModbusTransceiverBase : ITransceiver<ModbusMessage>
    {
        Rs232CommunicationManager rs232Manager;

        #region ITransceiver<ModbusMessage> implementation

        public abstract ModbusMessage ReceiveMessage(SerialPort port);

        public abstract void TransmitMessage(SerialPort port, ModbusMessage message);

        #endregion

        public ModbusTransceiverBase(ModbusConnectionParameters parameters)
        {
            rs232Manager = new Rs232CommunicationManager(parameters.GetConnectionParameters());
        }
    }
}
