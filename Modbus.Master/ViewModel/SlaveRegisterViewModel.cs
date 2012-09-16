using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SerialPortCommunicator.Modbus.Master.Model;
using SerialPortCommunicator.WpfHelpers;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class SlaveRegisterViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private string _value;
        private DelegateCommand _writeToSlaveCommand;
        private DelegateCommand _readFromSlaveCommand;
        private byte _slaveAddress;

        #endregion

        #region Properties

        public short Number { get; private set; }
        public string Value
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

        public ICommand ReadFromSlaveCommand
        {
            get
            {
                if (_readFromSlaveCommand == null)
                    _readFromSlaveCommand = new DelegateCommand(ReadFromSlave);
                return _readFromSlaveCommand;
            }
        }

        #endregion

        #region Constructors

        public SlaveRegisterViewModel(byte slaveAddress, short number)
        {
            _slaveAddress = slaveAddress;
            Number = number;
            Value = string.Empty;
        }

        public void ReadFromSlave()
        {
            MasterManager.Instance.BeginReadFromSlave(_slaveAddress, Number, result => Value = result);
        }

        #endregion
    }
}
