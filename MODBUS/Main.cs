using System;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using SerialPortCommunicator.Generics.Properties;
using SerialPortCommunicator.GUI;
using SerialPortCommunicator.Helpers;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Parameters;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generic.Parameters;
using SerialPortCommunicator.Modbus.Transceivers;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generic.Communicator;

namespace SerialPortCommunicator.Modbus
{
    public partial class Main : Form
    {
        private SerialPortCommunicator.Modbus.Communicator.CommunicationManager communicationManager;
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
            UpdateCommunicationManager();
            if (!communicationManager.OpenPort())
            {
                cmdClose_Click(this, new EventArgs());
                return;
            }
            ParametersControlsState(false);
        }

        private void UpdateCommunicationManager()
        {
            bool isAsciiSelected = asciiRadioButton.Checked;
            Parity parity;
            StopBits stopBits;

            if (e1RadioButton.Checked)
            {
                parity = Parity.Even;
                stopBits = StopBits.One;
            }
            else if (o1RadioButton.Checked)
            {
                parity = Parity.Odd;
                stopBits = StopBits.One;
            }
            else
            {
                parity = Parity.None;
                stopBits = StopBits.Two;
            }

            ConnectionParameters parameters = new ConnectionParameters(
                cboPort.Text, 
                int.Parse(cboBaud.Text),
                isAsciiSelected ? 7 : 8, 
                parity, 
                ((HandshakeMenuItem) cboHandshake.SelectedItem).type, 
                stopBits,
                isAsciiSelected ? EndMarker.CRLF : EndMarker.NONE);

            ITransceiver<RTUMessage> transceiver = new RTUTransceiver(new Transmitter(parameters), new Receiver(parameters));
            communicationManager = new SerialPortCommunicator.Modbus.Communicator.CommunicationManager(parameters, new ProgramWindow(rtbDisplay), transceiver);
            communicationManager.DataReceivedEvent += new DataReceivedEventHandler<RTUMessage>(OnDataReceived);
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
            communicationManager.WriteData(txtSend.Text);
            new ProgramWindow(rtbDisplay).displayMessage(txtSend.Text, MessageType.Outgoing);
            txtSend.Text = "";
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
            waitForPingAnswer = true;
            cmdSend.Enabled = false;
            pingContent = txtSend.Text;
            communicationManager.WriteData(pingQueryPrefix + pingContent);
            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = (int) pingTimeoutValue.Value;
            pingTimer.Elapsed += new ElapsedEventHandler(OnPingTimeout);
            pingTimer.Start();
        }

        private void OnPingTimeout(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();
            new ProgramWindow(rtbDisplay).displayMessage("Nie otrzymano odpowiedzi na ping", MessageType.Error);
            cmdSend.Invoke(new EventHandler(delegate
            {
                cmdSend.Enabled = true;
            }));
            waitForPingAnswer = false;
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs<RTUMessage> e)
        {
            if (waitForPingAnswer && e.Message.BinaryData.DeserializedString().Equals(pingAnswerPrefix + pingContent))
            {
                new ProgramWindow(rtbDisplay).displayMessage("Otrzymano odpowiedü na ping", MessageType.Incoming);
                cmdSend.Invoke(new EventHandler(delegate
                {
                    cmdSend.Enabled = true;
                }));
                waitForPingAnswer = false;
                pingTimer.Stop();
            }
            else if (e.Message.BinaryData.DeserializedString().StartsWith(pingQueryPrefix))
            {
                string query = e.Message.BinaryData.DeserializedString().Substring(pingQueryPrefix.Length);
                communicationManager.WriteData(pingAnswerPrefix + query);
                new ProgramWindow(rtbDisplay).displayMessage("Wys≥ano odpowiedü na ping", MessageType.Outgoing);
            }
            else
            {
                new ProgramWindow(rtbDisplay).displayMessage(e.Message.BinaryData.DeserializedString(), MessageType.Incoming);
            }
        }

    }
}