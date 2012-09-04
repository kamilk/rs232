using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.RS232.Communicator;

namespace SerialPortCommunicator.Modbus.MessageProcessors
{
    interface IModbusMessageProcessor
    {
        void SendMessage(Rs232CommunicationManager rs232Manager, ModbusMessage message);
        ModbusMessage ReceiveMessage(Rs232CommunicationManager rs232Manager);
    }
}
