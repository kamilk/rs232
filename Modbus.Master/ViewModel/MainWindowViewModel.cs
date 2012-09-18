using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.IO.Ports;
using SerialPortCommunicator.Modbus.Master.Model;
using SerialPortCommunicator.Modbus.CommonView;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private byte? _newSlaveAddress;
        private ObservableCollection<SlaveControlViewModel> _slaves;
        private string _errorMessages;

        #endregion

        #region Properties

        public byte? NewSlaveAddress
        {
            get { return _newSlaveAddress; }
            set
            {
                _newSlaveAddress = value;
                NotifyPropertyChanged("NewSlaveAddress");
            }
        }

        public ICommand AddSlaveCommand { get; private set; }

        public ICollectionView Slaves { get; private set; }

        public ICollectionView PortNames { get; private set; }

        public ModbusParityAndStopBits ParityAndStopBits { get; set; }

        public string ErrorMessages
        {
            get { return _errorMessages; }
            set
            {
                _errorMessages = value;
                NotifyPropertyChanged("ErrorMessages");
            }
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            AddSlaveCommand = new DelegateCommand(AddSlave);

            _slaves = new ObservableCollection<SlaveControlViewModel>();
            Slaves = CollectionViewSource.GetDefaultView(_slaves);

            PortNames = CollectionViewSource.GetDefaultView(SerialPort.GetPortNames());
            PortNames.MoveCurrentToFirst();

            ParityAndStopBits = ModbusParityAndStopBits.E1;

            MasterManager.Instance.RequestTimeout += new EventHandler<ModbusEventArgs>(OnRequestTimeout);
        }

        #endregion

        #region Methods

        private void AddSlave()
        {
            if (NewSlaveAddress == null)
                return;

            OpenPortIfClosed();

            if (!_slaves.Any(slave => slave.Address == NewSlaveAddress))
                _slaves.Add(new SlaveControlViewModel((byte)NewSlaveAddress));
        }

        private void OpenPortIfClosed()
        {
            if (!MasterManager.Instance.IsPortOpen)
            {
                MasterManager.Instance.OpenPort(new ModbusConnectionParameters()
                {
                    PortName = PortNames.CurrentItem as string,
                    BaudRate = 9600,
                    Handshake = Handshake.RequestToSend,
                    Mode = ModbusMode.Ascii,
                    ParityAndStopBits = ParityAndStopBits
                });
            }
        }

        void OnRequestTimeout(object sender, ModbusEventArgs e)
        {
            ErrorMessages += string.Format(
                "A request sent to slave {0}, register {1} has timed out.\n", 
                e.SlaveAddress, 
                e.RegisterNumber);
        }

        #endregion
    }
}
