using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.WpfHelpers;
using SerialPortCommunicator.Modbus.Slave.Model;
using System.Windows.Input;

namespace SerialPortCommunicator.Modbus.Slave.ViewModel
{
    class RegisterViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private short _currentValue;
        private short _requestedValue;
        private DelegateCommand _overrideValueCommand;

        #endregion

        #region Properties

        public short RegisterNumber { get; private set; }

        public short CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                NotifyPropertyChanged("CurrentValue");
            }
        }

        public short RequestedValue
        {
            get { return _requestedValue; }
            set
            {
                _requestedValue = value;
                NotifyPropertyChanged("RequestedValue");
            }
        }

        public ICommand OverrideValueCommand
        {
            get
            {
                if (_overrideValueCommand == null)
                {
                    _overrideValueCommand = new DelegateCommand(() =>
                        {
                            SlaveManager.Instance.SetRegisterValue(RegisterNumber, RequestedValue);
                        });
                }

                return _overrideValueCommand;
            }
        }

        #endregion

        #region Constructors

        public RegisterViewModel(short register)
        {
            RegisterNumber = register;

            short currentValue = SlaveManager.Instance.GetRegisterValue(register);
            CurrentValue = currentValue;
            RequestedValue = currentValue;

            SlaveManager.Instance.RegisterValueChanged += new EventHandler<RegisterValueChangedEventArgs>(OnRegisterValueChanged);
        }

        void OnRegisterValueChanged(object sender, RegisterValueChangedEventArgs e)
        {
            if (e.RegisterNumber == RegisterNumber)
                CurrentValue = e.NewValue;
        }

        #endregion
    }
}
