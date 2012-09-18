using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class ModbusDataReadEventArgs : EventArgs
    {
        public byte SlaveAddress { get; private set; }
        public short RegisterNumber { get; private set; }
        public string ReadData { get; private set; }

        public ModbusDataReadEventArgs(byte slaveAddress, short register, string data)
        {
            SlaveAddress = slaveAddress;
            RegisterNumber = register;
            ReadData = data; 
        }
    }
}
