using System;
using System.IO.Ports;
using SerialPortCommunicator.Generics.Helpers;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics.Transceivers.Parameters;
using System.Collections.Generic;

namespace SerialPortCommunicator.Generics
{
    public abstract class CommunicationManager<TMessage>
    {
        #region Manager Variables
        //property variables
        protected ConnectionParameters ConnectionParameters { get; set; }
        //global manager variables
        protected SerialPort comPort = new SerialPort();

        protected ITransceiver<TMessage> Transceiver { set; get; }

        #endregion

        #region Events

        public event EventHandler<DataReceivedEventArgs<TMessage>> DataReceivedEvent;
        public event EventHandler<LogEventArgs> LoggableEventOccurred;

        #endregion

        public CommunicationManager(ConnectionParameters connectionParameters, ITransceiver<TMessage> transceiver)
        {
            ConnectionParameters = connectionParameters;
            Transceiver = transceiver;
            comPort.DataReceived += new SerialDataReceivedEventHandler(ComPortDataReceived);
        }

        public abstract void WriteData(string msg);

        public bool OpenPort()
        {
            try
            {
                if (comPort.IsOpen)
                    comPort.Close();

                comPort.BaudRate = ConnectionParameters.BaudRate;
                comPort.DataBits = ConnectionParameters.DataBits;
                comPort.StopBits = ConnectionParameters.StopBits;
                comPort.Parity = ConnectionParameters.Parity;
                comPort.PortName = ConnectionParameters.PortName;
                comPort.Handshake = ConnectionParameters.Handshake;
                comPort.DtrEnable = true;
                comPort.RtsEnable = true;
                comPort.WriteBufferSize = 1024;
                comPort.ReadBufferSize = 2048;
                comPort.Open();
                NotifyLoggableMessageOccurred("Port opened at " + DateTime.Now, MessageType.Normal);
                return true;
            }
            catch (Exception ex)
            {
                NotifyLoggableMessageOccurred(ex.Message, MessageType.Error);
                return false;
            }
        }

        public void ClosePort()
        {
            comPort.Close();
            NotifyLoggableMessageOccurred("Port closed at " + DateTime.Now, MessageType.Normal);
        }

        protected virtual void ComPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (comPort.BytesToRead > 0)
            {
                IEnumerable<TMessage> receivedMessages = Transceiver.ReceiveMessages(comPort);
                foreach (TMessage message in receivedMessages)
                    fireDataReceivedEvent(message);
            }
        }

        protected void fireDataReceivedEvent(TMessage receivedMessage)
        {
            if (DataReceivedEvent != null)
                DataReceivedEvent(this, new DataReceivedEventArgs<TMessage>(receivedMessage));
        }

        protected void NotifyLoggableMessageOccurred(string message, MessageType type = MessageType.Normal)
        {
            if (LoggableEventOccurred != null)
                LoggableEventOccurred(this, new LogEventArgs(message, type));
        }
    }
}
