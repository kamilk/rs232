using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Modbus.CommonView;
using SerialPortCommunicator.Modbus.Slave.Model;
using System.Windows.Input;

namespace SerialPortCommunicator.Modbus.Slave.ViewModel
{
    class RegisterViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private string _currentValue;
        private string _requestedValue;
        private DelegateCommand _overrideValueCommand;

        #endregion

        #region Properties

        public short RegisterNumber { get; private set; }

        public string CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                NotifyPropertyChanged("CurrentValue");
            }
        }

        public string RequestedValue
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

            string currentValue = SlaveManager.Instance.GetRegisterValue(register);
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
