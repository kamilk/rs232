using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class RequestTimeoutEventArgs : ModbusEventArgs
    {
        public int AttemptsLeft { get; private set; }

        public RequestTimeoutEventArgs(byte slaveAddress, short register, int attemptsLeft)
            : base(slaveAddress, register)
        {
            AttemptsLeft = attemptsLeft;
        }
    }
}
