using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class ModbusEventArgs : EventArgs
    {
        public byte SlaveAddress { get; private set; }
        public short RegisterNumber { get; private set; }

        public ModbusEventArgs(byte slaveAddress, short register)
        {
            this.SlaveAddress = slaveAddress;
            this.RegisterNumber = register;
        }
    }
}
