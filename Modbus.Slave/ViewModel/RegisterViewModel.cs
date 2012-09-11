using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.WpfHelpers;
using SerialPortCommunicator.Modbus.Slave.Model;

namespace SerialPortCommunicator.Modbus.Slave.ViewModel
{
    class RegisterViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        public short _currentValue;
        public short _requestedValue;

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
