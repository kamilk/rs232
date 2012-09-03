using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generic;
using SerialPortCommunicator.RS232.Communicator;

namespace SerialPortCommunicator.Modbus
{
    class ModbusCommunicationManager
    {
        public event EventHandler<DataReceivedEventArgs<ModbusMessage>> MessageReceived;

        public void OpenPort(ModbusConnectionParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void ClosePort()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(ModbusMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
