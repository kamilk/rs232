using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class ModbusRequest
    {
        public ModbusMessage Message { get; private set; }
        public byte SlaveAddress { get; private set; }
        public short RegisterNumber { get; private set; }
        public Action<ModbusRequest, ModbusMessage> ResponseHandler { get; set; }

        public event EventHandler TimeoutEvent;

        public ModbusRequest(ModbusMessage message, byte slaveAddress, short register)
        {
            Message = message;
            SlaveAddress = slaveAddress;
            RegisterNumber = register;
        }

        public void StartTimer()
        {
            //TODO
        }

        internal void StopTimer()
        {
            //TODO
        }
    }
}
