using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.IO.Ports;
using SerialPortCommunicator.Modbus.Master.Model;
using SerialPortCommunicator.WpfHelpers;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private byte? _newSlaveAddress;
        private ObservableCollection<SlaveControlViewModel> _slaves;

        #endregion

        #region Properties for data binding

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

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            AddSlaveCommand = new DelegateCommand(AddSlave);

            _slaves = new ObservableCollection<SlaveControlViewModel>();
            Slaves = CollectionViewSource.GetDefaultView(_slaves);
        }

        #endregion

        #region Methods

        private void AddSlave()
        {
            if (!MasterManager.Instance.IsPortOpen)
            {
                MasterManager.Instance.OpenPort(new ModbusConnectionParameters()
                {
                    PortName = "COM7",
                    BaudRate = 9600,
                    Handshake = Handshake.RequestToSend,
                    Mode = ModbusMode.Ascii,
                    ParityAndStopBits = ModbusParityAndStopBits.E1
                });
            }

            if (!_slaves.Any(slave => slave.Address == NewSlaveAddress))
                _slaves.Add(new SlaveControlViewModel((byte)NewSlaveAddress));
        }

        #endregion
    }
}
