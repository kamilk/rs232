using System;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using SerialPortCommunicator.Generics.Properties;
using SerialPortCommunicator.Helpers;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Parameters;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generic.Parameters;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Modbus.Transceivers;
using SerialPortCommunicator.Generic;
using SerialPortCommunicator.Generic.Helpers;

namespace SerialPortCommunicator.Modbus
{
    public partial class Main : Form
    {
        private ModbusCommunicationManager communicationManager;
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
            //TODO zaimplementować wysyłanie wiadomości
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
            txtSend.Enabled = !enable;
        }

        private void pingButton_Click(object sender, EventArgs e)
        {
            //TODO zaimplementować ping (albo wyrzucić przycisk)
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
            //TODO zaimplementować odbieranie wiadomości
        }

    }
}