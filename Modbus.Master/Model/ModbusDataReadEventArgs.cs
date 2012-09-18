using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class ModbusDataReadEventArgs : ModbusEventArgs
    {
        public string ReadData { get; private set; }

        public ModbusDataReadEventArgs(byte slaveAddress, short register, string data)
            : base(slaveAddress, register)
        {
            ReadData = data; 
        }
    }
}
