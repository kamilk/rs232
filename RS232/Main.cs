using System;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using SerialPortCommunicator.Generics.Properties;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.RS232.Communicator;
using SerialPortCommunicator.RS232.Parameters;
using SerialPortCommunicator.RS232.Transceivers;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generic.Parameters;
using SerialPortCommunicator.Helpers;
using SerialPortCommunicator.Generic;
using SerialPortCommunicator.Generic.Helpers;

namespace SerialPortCommunicator.RS232
{
    public partial class Main : Form
    {
        private Rs232CommunicationManager communicationManager;
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
            ConnectionParameters parameters = new ConnectionParameters(
                cboPort.Text, 
                Int32.Parse(cboBaud.Text),
                Int32.Parse(cboData.Text), 
                (Parity)cboParity.SelectedItem, 
                ((HandshakeMenuItem) cboHandshake.SelectedItem).type, 
                (StopBits)cboStop.SelectedItem,
                ((EndMarkerMenuItem) cboEndMarker.SelectedItem).type);

            ITransceiver<RS232Message> transceiver = new RS232Transceiver(new Transmitter(parameters), new Receiver(parameters));
            communicationManager = new Rs232CommunicationManager(parameters, transceiver);
            communicationManager.LoggableEventOccurred += new EventHandler<LogEventArgs>(communicationManager_LoggableEventOccurred);
            communicationManager.DataReceivedEvent += OnDataReceived;
        }

        /// <summary>
        /// Method to initialize serial port
        /// values to standard defaults
        /// </summary>
        private void SetDefaults()
        {
            if (cboPort.Items.Count > 0)
                cboPort.SelectedIndex = 1;
            cboBaud.SelectedIndex = 5;
            cboParity.SelectedIndex = 0;
            cboStop.SelectedIndex = 1;
            cboData.SelectedIndex = 1;
            cboHandshake.SelectedIndex = 2;
            cboEndMarker.SelectedIndex = 2;
            pingTimeoutValue.Text = "100";
        }

        /// <summary>
        /// methos to load our serial
        /// port option values
        /// </summary>
        private void LoadValues()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                cboPort.Items.Add(portName);
            }
            foreach (StopBits sb in Enum.GetValues(typeof(StopBits)))
            {
                cboStop.Items.Add(sb);
            }
            foreach (Parity p in Enum.GetValues(typeof(Parity)))
            {
                cboParity.Items.Add(p);
            }
            foreach (Handshake h in Enum.GetValues(typeof(Handshake)))
            {
                cboHandshake.Items.Add(new HandshakeMenuItem(h));
            }
            foreach (EndMarker e in Enum.GetValues(typeof(EndMarker)))
            {
                cboEndMarker.Items.Add(new EndMarkerMenuItem(e));
            }
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            communicationManager.WriteData(txtSend.Text);
            rtbDisplay.InvokeDisplayMessage(txtSend.Text, MessageType.Outgoing);
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
            cboData.Enabled = enable;
            cboStop.Enabled = enable;
            cboParity.Enabled = enable;
            cboHandshake.Enabled = enable;
            cboEndMarker.Enabled = enable;
            pingLabel.Enabled = !enable;
            cmdOpen.Enabled = enable;
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
            rtbDisplay.InvokeDisplayMessage("Nie otrzymano odpowiedzi na ping", MessageType.Error);
            cmdSend.Invoke(new EventHandler(delegate
            {
                cmdSend.Enabled = true;
            }));
            waitForPingAnswer = false;
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs<RS232Message> e)
        {
            if (waitForPingAnswer && e.Message.BinaryData.DeserializedString().Equals(pingAnswerPrefix + pingContent))
            {
                rtbDisplay.InvokeDisplayMessage("Otrzymano odpowiedü na ping", MessageType.Incoming);
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
                rtbDisplay.InvokeDisplayMessage("Wys≥ano odpowiedü na ping", MessageType.Outgoing);
            }
            else
            {
                rtbDisplay.InvokeDisplayMessage(e.Message.MessageString, MessageType.Incoming);
            }
        }

        private void communicationManager_LoggableEventOccurred(object sender, LogEventArgs e)
        {
            rtbDisplay.InvokeDisplayMessage(e.Message, e.MessageType);
        }
    }
}