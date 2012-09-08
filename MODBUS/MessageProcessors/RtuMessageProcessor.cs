using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Transceivers;

namespace SerialPortCommunicator.Modbus.MessageProcessors
{
    class RtuMessageProcessor : IModbusMessageProcessor
    {
        public void SendMessage(Rs232CommunicationManager rs232Manager, ModbusMessage message)
        {
            throw new NotImplementedException();
        }

        public ModbusMessage ProcessMessage(RS232Message rs232Message)
        {
            throw new NotImplementedException();
        }
    }
}
