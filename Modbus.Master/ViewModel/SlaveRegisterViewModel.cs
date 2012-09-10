using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class SlaveRegisterViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private short _value;

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

        #endregion

        #region Constructors

        public SlaveRegisterViewModel(short number)
        {
            Number = number;
            Value = 0;
        }

        #endregion
    }
}
