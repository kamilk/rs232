using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

namespace SerialPortCommunicator.Modbus.Master.ViewModel
{
    class SlaveControlViewModel : NotifyPropertyChangedBase
    {
        #region Properites for data binding

        public byte Address { get; set; }
        public IEnumerable<SlaveRegisterViewModel> Registers { get; private set; }

        #endregion

        #region Constructors

        public SlaveControlViewModel(byte address)
        {
            Address = address;

            var registers = new List<SlaveRegisterViewModel>();
            for (byte i = 0; i < 5; i++)
                registers.Add(new SlaveRegisterViewModel(address, i));
            Registers = registers;
        }

        #endregion
    }
}
