using System;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Threading;

using SerialPortCommunicator;
using SerialPortCommunicator.Helpers;
using SerialPortCommunicator.GUI;
using SerialPortCommunicator.Properties;
using SerialPortCommunicator.Transceivers;

namespace SerialPortCommunicator.Communicator
{
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(Message receivedMessage)
        {
            Message = receivedMessage;
        }
        public Message Message { get; private set; }
    }

    class CommunicationManager
    {
        #region Manager Variables
        //property variables
        private ConnectionParameters ConnectionParameters { get; set; }
        //global manager variables
        private SerialPort comPort = new SerialPort();
        private bool received;
        private ProgramWindow ProgramWindow { set; get; }
        private ITransceiver Transceiver { set; get; }

        public event DataReceivedEventHandler DataReceivedEvent;
        #endregion

        public CommunicationManager(ConnectionParameters connectionParameters, ProgramWindow programWindow, ITransceiver transceiver)
        {
            ConnectionParameters = connectionParameters;
            ProgramWindow = programWindow;
            Transceiver = transceiver;
            comPort.DataReceived += new SerialDataReceivedEventHandler(ComPortDataReceived);
        }

        #region WriteData
        public void WriteData(string msg)
        {
            Message message = new SerialPortCommunicator.Transceivers.Message(0, 0, msg.SerializedString());
            try
            {
                Transceiver.TransmitData(comPort, message);
            }
            catch (MessageException ex)
            {
                ProgramWindow.displayMessage("B³¹d przy wysy³aniu wiadomoœci: " + ex.Message, MessageType.Error);
            }
        }
        #endregion

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

        /// <summary>
        /// method that will be called when theres data waiting in the buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ComPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Message receivedMessage = Transceiver.DataReceived(comPort);
                DataReceivedEvent(this, new DataReceivedEventArgs(receivedMessage));
            }
            catch (MessageException ex)
            {
                ProgramWindow.displayMessage("B³¹d przy odbieraniu wiadomoœci: " + ex.Message, MessageType.Error);
            }
        }

        public void WaitForResponse(int timeout)
        {
            DateTime future = DateTime.Now;
            future = future.AddSeconds(timeout);
            while (DateTime.Now.TimeOfDay < future.TimeOfDay)
            {
                if (received)
                {
                    received = false;
                    return;
                }
            }
            if (!received)
                ProgramWindow.displayMessage("Response didn't come!", MessageType.Error);
            received = false;
        }
    }
}
