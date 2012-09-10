using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Data;
using SerialPortCommunicator.WpfHelpers;

namespace SerialPortCommunicator.Modbus.Slave.ViewModel
{
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Fields

        private string[] _portNames;
        private int _address;

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

        public int Address
        {
            get { return _address; }
            set
            {
                _address = value;
                NotifyPropertyChanged("Address");
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            Address = 1;
        }
    }
}
