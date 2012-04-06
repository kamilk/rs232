using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using SerialPortCommunicator.Properties;
using System.IO.Ports;
using SerialPortCommunicator.GUI;
using RS232.Transceivers;
using SerialPortCommunicator.Helpers;
using System.Timers;
using SerialPortCommunicator.Communicator;
using RS232.Parameters;

namespace SerialPortCommunicator.RS232
{
    public partial class Main : Form
    {
        private CommunicationManager communicationManager;
        private Boolean waitForPingAnswer { get; set; }
        private System.Timers.Timer pingTimer { get; set; }
        private string pingTextQuery = "!#@$#%$^%&^*&(*)(_)+";
        private string pingTextAnswer = "asdfghjk";

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
            ConnectionParameters parameters = new ConnectionParameters(cboPort.Text, Int16.Parse(cboBaud.Text),
                Int16.Parse(cboData.Text), (Parity)cboParity.SelectedItem, Handshake.None, (StopBits)cboStop.SelectedItem,
                ((XONTypeMenuItem) cboXON.SelectedItem).type, ((EndMarkerMenuItem) cboEndMarker.SelectedItem).type);

            ITransceiver transceiver = new Transceiver(new Transmitter(parameters), new Receiver(parameters));
            communicationManager = new CommunicationManager(parameters, new ProgramWindow(rtbDisplay), transceiver);
            communicationManager.DataReceivedEvent += new DataReceivedEventHandler(OnDataReceived);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (waitForPingAnswer && e.Message.Data.DeserializedString().Equals(pingTextAnswer))
            {
                new ProgramWindow(rtbDisplay).displayMessage("Otrzymano odpowiedü na ping", MessageType.Incoming);
                cmdSend.Invoke(new EventHandler(delegate
                {
                    cmdSend.Enabled = true;
                }));
                waitForPingAnswer = false;
                pingTimer.Stop();
            }
            else if (e.Message.Data.DeserializedString().Equals(pingTextQuery))
            {
                communicationManager.WriteData(pingTextAnswer);
                new ProgramWindow(rtbDisplay).displayMessage("Wys≥ano odpowiedü na ping", MessageType.Outgoing);
            }
            else
            {
                new ProgramWindow(rtbDisplay).displayMessage(e.Message.Data.DeserializedString(), MessageType.Incoming);
            }
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
            cboXON.SelectedIndex = 2;
            cboEndMarker.SelectedIndex = 1;
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
            foreach (XONType x in Enum.GetValues(typeof(XONType)))
            {
                cboXON.Items.Add(new XONTypeMenuItem(x));
            }
            foreach (EndMarker e in Enum.GetValues(typeof(EndMarker)))
            {
                cboEndMarker.Items.Add(new EndMarkerMenuItem(e));
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
            cboData.Enabled = enable;
            cboStop.Enabled = enable;
            cboParity.Enabled = enable;
            cboXON.Enabled = enable;
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
            communicationManager.WriteData(pingTextQuery);
            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = (int) pingTimeoutValue.Value;
            pingTimer.Elapsed += new ElapsedEventHandler(OnPingTimeout);
            pingTimer.Start();
        }

        private void OnPingTimeout(object sender, ElapsedEventArgs e)
        {
            new ProgramWindow(rtbDisplay).displayMessage("Nie otrzymano odpowiedzi na ping", MessageType.Error);
            pingTimer.Stop();
            cmdSend.Invoke(new EventHandler(delegate
            {
                cmdSend.Enabled = true;
            }));
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int timeout = 10;
            try
            {
                timeout = int.Parse(pingTimeoutValue.Text);
            }
            catch (Exception)
            { }
            communicationManager.WaitForResponse(timeout);
            cmdSend.Invoke(new EventHandler(delegate
            {
                Enabled = true;
            }));

        }


    }
}