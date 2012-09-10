using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using SerialPortCommunicator.Modbus.Master.ViewModel.Helpers;
using System.IO.Ports;
using SerialPortCommunicator.Modbus.Master.Model;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private int? newSlaveAddress;

        #endregion

        #region Properties for data binding

        public int? NewSlaveAddress
        {
            get { return newSlaveAddress; }
            set
            {
                newSlaveAddress = value;
                NotifyPropertyChanged("NewSlaveAddress");
            }
        }

        public ICommand AddSlaveCommand { get; private set; }

        public SlaveControlViewModel Slave { get; private set; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            AddSlaveCommand = new DelegateCommand(AddSlave);
            Slave = new SlaveControlViewModel(1);
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
        }

        #endregion
    }
}
