using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Communicator;

namespace SerialPortCommunicator.Modbus.MessageProcessors
{
    class RtuMessageProcessor : IModbusMessageProcessor
    {
        public void SendMessage(Rs232CommunicationManager rs232Manager, ModbusMessage message)
        {
            throw new NotImplementedException();
        }

        public ModbusMessage ReceiveMessage(Rs232CommunicationManager rs232Manager)
        {
            throw new NotImplementedException();
        }
    }
}
