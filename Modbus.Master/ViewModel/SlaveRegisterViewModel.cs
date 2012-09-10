using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Modbus.Master.ViewModel.Helpers;
using System.Windows.Input;
using SerialPortCommunicator.Modbus.Master.Model;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class SlaveRegisterViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private short _value;
        private DelegateCommand _writeToSlaveCommand;
        private byte _slaveAddress;

        #endregion

        #region Properties

        public short Number { get; private set; }
        public short Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        public ICommand WriteToSlaveCommand
        {
            get
            {
                if (_writeToSlaveCommand == null)
                    _writeToSlaveCommand = new DelegateCommand(() => 
                        MasterManager.Instance.WriteToSlave(_slaveAddress, Number, Value));
                return _writeToSlaveCommand;
            }
        }

        #endregion

        #region Constructors

        public SlaveRegisterViewModel(byte slaveAddress, short number)
        {
            _slaveAddress = slaveAddress;
            Number = number;
            Value = 0;
        }

        #endregion
    }
}
