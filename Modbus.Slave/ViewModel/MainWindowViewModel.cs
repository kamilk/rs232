﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Data;
using SerialPortCommunicator.Modbus.CommonView;
using System.Windows.Input;
using SerialPortCommunicator.Modbus.Slave.Model;

namespace SerialPortCommunicator.Modbus.Slave.ViewModel
{
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private string[] _portNames;
        private byte _address;

        #endregion

        #region Properties

        public ICollectionView PortNames
        {
            get
            {
                if (_portNames == null)
                    _portNames = SerialPort.GetPortNames();
                return CollectionViewSource.GetDefaultView(_portNames);
            }
        }

        public byte Address
        {
            get { return _address; }
            set
            {
                _address = value;
                NotifyPropertyChanged("Address");
            }
        }

        public ModbusParityAndStopBits ParityAndStopBits { get; set; }

        public IEnumerable<RegisterViewModel> Registers { get; private set; }

        public ICommand OpenPortCommand { get; private set; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            Address = 1;
            OpenPortCommand = new DelegateCommand(OpenPort);

            var registers = new List<RegisterViewModel>();
            for (short i = 0; i < SlaveManager.NumberOfRegisters; i++)
                registers.Add(new RegisterViewModel(i));
            Registers = registers;
        }

        #endregion

        #region Methods

        private void OpenPort()
        {
            SlaveManager.Instance.Address = Address;
            SlaveManager.Instance.OpenPort(new ModbusConnectionParameters()
            {
                BaudRate = 9600,
                Handshake = Handshake.RequestToSend,
                Mode = ModbusMode.Ascii,
                ParityAndStopBits = ParityAndStopBits,
                PortName = (string)PortNames.CurrentItem
            });
        }

        #endregion
    }
}
