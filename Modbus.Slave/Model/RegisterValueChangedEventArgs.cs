using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Slave.Model
{
    class RegisterValueChangedEventArgs : EventArgs
    {
        public short RegisterNumber { get; private set; }
        public string NewValue { get; private set; }

        public RegisterValueChangedEventArgs(short registerNumber, string newValue)
        {
            RegisterNumber = registerNumber;
            NewValue = newValue;
        }
    }
}
