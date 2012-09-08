using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.Modbus.MessageProcessors;
using SerialPortCommunicator.RS232.Transceivers;

namespace SerialPortCommunicator.Modbus
{
    class ModbusCommunicationManager
    {
        Rs232CommunicationManager rs232Manager;
        IModbusMessageProcessor messageProcessor;

        public event EventHandler<DataReceivedEventArgs<ModbusMessage>> MessageReceived;

        public void OpenPort(ModbusConnectionParameters parameters)
        {
            if (rs232Manager != null)
            {
                rs232Manager.ClosePort();
                rs232Manager.DataReceivedEvent -= OnDataReceived;
            }

            rs232Manager = new Rs232CommunicationManager(parameters.GetConnectionParameters());
            rs232Manager.DataReceivedEvent += OnDataReceived;
            rs232Manager.OpenPort();

            messageProcessor = parameters.GetMessageProcessor();
        }

        public void ClosePort()
        {
            if (rs232Manager != null)
            {
                rs232Manager.ClosePort();
                rs232Manager = null;
            }
        }

        public void SendMessage(ModbusMessage message)
        {
            messageProcessor.SendMessage(rs232Manager, message);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs<RS232Message> e)
        {
            ModbusMessage message = messageProcessor.ProcessMessage(e.Message);
        }
    }
}
