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
using RS232.Transceivers;

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
            //msg = @"In telecommunications, RS-232 is the traditional name for a series of standards for serial binary single-ended data and control signals connecting between a DTE (Data Terminal Equipment) and a DCE (Data Circuit-terminating Equipment). It is commonly used in computer serial ports. The standard defines the electrical characteristics and timing of signals, the meaning of signals, and the physical size and pin out of connectors. The current version of the standard is TIA-232-F Interface Between Data Terminal Equipment and Data Circuit-Terminating Equipment Employing Serial Binary Data Interchange, issued in 1997.An RS-232 port was once a standard feature of a personal computer for connections to modems, printers, mice, data storage, un-interruptible power supplies, and other peripheral devices. However, the limited transmission speed, relatively large voltage swing, and large standard connectors motivated development of the universal serial bus which has displaced RS-232 from most of its peripheral interface roles. Many modern personal computers have no RS-232 ports and must use an external converter to connect to older peripherals. Some RS-232 devices are still found especially in industrial machines or scientific instruments.Contents  [hide] 1 Scope of the standard2 History3 Limitations of the standard4 Role in modern personal computers5 Standard details5.1 Voltage levels5.2 Connectors5.3 Signals5.4 Cables6 Conventions6.1 RTS/CTS handshaking6.2 3-wire and 5-wire RS-2327 Seldom used features.1 Signal rate selection7.2 Loopback testing7.3 Timing signals7.4 Secondary channel8 Related standards9 Development tools10 References[edit]Scope of the standardThe Electronic Industries Association (EIA) standard RS-232-C[1] as of 1969 defines:Electrical signal characteristics such as voltage levels, signaling rate, timing and slew-rate of signals, voltage withstand level, short-circuit behavior, and maximum load capacitance.Interface mechanical characteristics, pluggable connectors and pin identification.Functions of each circuit in the interface connector.Standard subsets of interface circuits for selected telecom applications.The standard does not define such elements as the character encoding or the framing of characters, or error detection protocols. The standard does not define bit rates for transmission, except that it says it is intended for bit rates lower than 20,000 bits per second. Many modern devices support speeds of 115,200 bit/s and above. RS 232 makes no provision for power to peripheral devices.Details of character format and transmission bit rate are controlled by the serial port hardware, often a single integrated circuit called a UART that converts data from parallel to asynchronous start-stop serial form. Details of voltage levels, slew rate, and short-circuit behavior are typically controlled by a line driver that converts from the UART's logic levels to RS-232 compatible signal levels, and a receiver that converts from RS-232 compatible signal levels to the UART's logic levels.";
            Transceiver.TransmitMessage(comPort, new Message(msg.SerializedString()));
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

        void ComPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Message receivedMessage = Transceiver.ReceiveMessage(comPort);
            DataReceivedEvent(this, new DataReceivedEventArgs(receivedMessage));
        }

    }
}
