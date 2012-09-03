using System;
using System.IO.Ports;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generics.Properties;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.GUI;

namespace SerialPortCommunicator.Generic.Communicator
{
    public delegate void DataReceivedEventHandler<TMessage>(object sender, DataReceivedEventArgs<TMessage> e);

    public class DataReceivedEventArgs<TMessage> : EventArgs
    {
        public DataReceivedEventArgs(TMessage receivedMessage)
        {
            Message = receivedMessage;
        }
        public TMessage Message { get; private set; }
    }

    public abstract class CommunicationManager<TMessage>
    {
        #region Manager Variables
        //property variables
        protected ConnectionParameters ConnectionParameters { get; set; }
        //global manager variables
        protected SerialPort comPort = new SerialPort();

        protected ProgramWindow ProgramWindow { set; get; }
        protected ITransceiver<TMessage> Transceiver { set; get; }

        public event DataReceivedEventHandler<TMessage> DataReceivedEvent;
        #endregion

        public CommunicationManager(ConnectionParameters connectionParameters, ProgramWindow programWindow, ITransceiver<TMessage> transceiver)
        {
            ConnectionParameters = connectionParameters;
            ProgramWindow = programWindow;
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
                ProgramWindow.displayMessage("Port opened at " + DateTime.Now, MessageType.Normal);
                return true;
            }
            catch (Exception ex)
            {
                ProgramWindow.displayMessage(ex.Message, MessageType.Error);
                return false;
            }
        }

        public void ClosePort()
        {
            comPort.Close();
            ProgramWindow.displayMessage("Port closed at " + DateTime.Now, MessageType.Normal);
        }

        protected virtual void ComPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            TMessage receivedMessage = Transceiver.ReceiveMessage(comPort);
            fireDataReceivedEvent(receivedMessage);
        }

        protected void fireDataReceivedEvent(TMessage receivedMessage)
        {
            if (DataReceivedEvent != null)
                DataReceivedEvent(this, new DataReceivedEventArgs<TMessage>(receivedMessage));
        }

    }
}
