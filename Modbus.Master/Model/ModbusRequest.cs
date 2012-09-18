using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class ModbusRequest
    {
        public ModbusMessage Message { get; private set; }
        public int SlaveAddress { get; private set; }
        public Action<ModbusMessage> ResponseHandler { get; set; }

        public event EventHandler TimeoutEvent;

        public ModbusRequest(ModbusMessage message, int slaveAddress)
        {
            Message = message;
            SlaveAddress = slaveAddress;
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
