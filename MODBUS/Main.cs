using System;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using SerialPortCommunicator.Generics.Helpers;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Parameters;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics;

namespace SerialPortCommunicator.Modbus
{
    public partial class Main : Form
    {
        private ModbusCommunicationManager communicationManager = new ModbusCommunicationManager();
        private Boolean waitForPingAnswer { get; set; }
        private System.Timers.Timer pingTimer { get; set; }
        private string pingQueryPrefix = "!(*^^(&$%*)(!@#";
        private string pingContent = "";
        private string pingAnswerPrefix = "!#@$#%$^%&^*&(*)(_)+";

        string _transType = string.Empty;
        public Main()
        {
            waitForPingAnswer = false;
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadValues();
            SetDefaults();
            ParametersControlsState(true);
        }

        private void cmdOpen_Click(object sender, EventArgs e)
        {
            if (communicationManager != null)
                communicationManager.ClosePort();
            OpenPort();
            ParametersControlsState(false);
        }

        private void OpenPort()
        {
            ModbusParityAndStopBits parityAndStopBits;
            if (e1RadioButton.Checked)
                parityAndStopBits = ModbusParityAndStopBits.E1;
            else if (o1RadioButton.Checked)
                parityAndStopBits = ModbusParityAndStopBits.O1;
            else
                parityAndStopBits = ModbusParityAndStopBits.N2;

            var parameters = new ModbusConnectionParameters()
            {
                Mode = asciiRadioButton.Checked ? ModbusMode.Ascii : ModbusMode.Rtu,
                PortName = cboPort.Text,
                BaudRate = int.Parse(cboBaud.Text),
                Handshake = ((HandshakeMenuItem)cboHandshake.SelectedItem).type,
                ParityAndStopBits = parityAndStopBits,
            };

            communicationManager.OpenPort(parameters);
        }

        /// <summary>
        /// Method to initialize serial port
        /// values to standard defaults
        /// </summary>
        private void SetDefaults()
        {
            if (cboPort.Items.Count > 0)
                cboPort.SelectedIndex = 2;
            cboBaud.SelectedIndex = 5;
            asciiRadioButton.Checked = true;
            rtuRadioButton.Checked = false;
            cboHandshake.SelectedIndex = 2;
            pingTimeoutValue.Text = "100";
        }

        /// <summary>
        /// methods to load our serial
        /// port option values
        /// </summary>
        private void LoadValues()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                cboPort.Items.Add(portName);
            }
            foreach (Handshake h in Enum.GetValues(typeof(Handshake)))
            {
                cboHandshake.Items.Add(new HandshakeMenuItem(h));
            }
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            var message = new ModbusMessage(
                txtMessageText.Text,
                Convert.ToByte(txtAddress.Text),
                Convert.ToByte(txtFunction.Text, 16));

            communicationManager.SendMessage(message);
            InvokeDisplayModbusMessage(message, MessageType.Outgoing);
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            communicationManager.ClosePort();
            ParametersControlsState(true);
        }

        private void ParametersControlsState(bool enable)
        {
            cboPort.Enabled = enable;
            cboBaud.Enabled = enable;
            cboHandshake.Enabled = enable;
            asciiRadioButton.Enabled = enable;
            rtuRadioButton.Enabled = enable;
            cmdOpen.Enabled = enable;

            pingLabel.Enabled = !enable;
            cmdClose.Enabled = !enable;
            cmdSend.Enabled = !enable;
            pingButton.Enabled = !enable;
            txtMessageText.Enabled = !enable;
        }

        private void pingButton_Click(object sender, EventArgs e)
        {
            //TODO zaimplementowaæ ping (albo wyrzuciæ przycisk)
        }

        private void OnPingTimeout(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();
            rtbDisplay.InvokeDisplayMessage("Nie otrzymano odpowiedzi na ping", MessageType.Error);
            cmdSend.Invoke(new EventHandler(delegate
            {
                cmdSend.Enabled = true;
            }));
            waitForPingAnswer = false;
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            //TODO zaimplementowaæ odbieranie wiadomoœci
        }

        private void InvokeDisplayModbusMessage(ModbusMessage message, MessageType type)
        {
            string stringToDisplay = string.Format(
                "Address: {0} Function: {1:X} Message: {2}",
                message.Address,
                message.Function,
                message.MessageHexString);
            rtbDisplay.InvokeDisplayMessage(stringToDisplay, type);
        }
    }
}