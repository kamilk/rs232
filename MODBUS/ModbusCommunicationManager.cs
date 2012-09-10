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
    public class ModbusCommunicationManager
    {
        #region Fields

        Rs232CommunicationManager rs232Manager;
        IModbusMessageProcessor messageProcessor;

        #endregion

        #region Properties

        public bool IsPortOpen { get; private set; }

        #endregion

        #region Events

        public event EventHandler<DataReceivedEventArgs<ModbusMessage>> MessageReceived;

        #endregion

        #region Constructors

        public ModbusCommunicationManager()
        {
            IsPortOpen = false;
        }

        #endregion

        #region Methods

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

            IsPortOpen = true;
        }

        public void ClosePort()
        {
            if (rs232Manager != null)
            {
                rs232Manager.ClosePort();
                rs232Manager = null;
            }

            IsPortOpen = false;
        }

        public void SendMessage(ModbusMessage message)
        {
            messageProcessor.SendMessage(rs232Manager, message);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs<RS232Message> e)
        {
            ModbusMessage message = messageProcessor.ProcessMessage(e.Message);
            if (MessageReceived != null)
                MessageReceived(this, new DataReceivedEventArgs<ModbusMessage>(message));
        }

        #endregion
    }
}
