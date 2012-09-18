using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class ModbusRequest
    {
        #region Fields

        public Timer _timer;

        #endregion

        #region Properties

        public ModbusMessage Message { get; private set; }
        public byte SlaveAddress { get; private set; }
        public short RegisterNumber { get; private set; }
        public Action<ModbusRequest, ModbusMessage> ResponseHandler { get; set; }

        #endregion

        #region Events

        public event EventHandler TimeoutEvent;

        #endregion

        #region Constructors

        public ModbusRequest(ModbusMessage message, byte slaveAddress, short register)
        {
            Message = message;
            SlaveAddress = slaveAddress;
            RegisterNumber = register;
        }

        #endregion

        #region Methods

        public void StartTimer()
        {
            _timer = new Timer(OnTimeout, null, 5000, Timeout.Infinite);
        }

        public void StopTimer()
        {
            _timer.Dispose();
            _timer = null;
        }

        private void OnTimeout(object state)
        {
            if (TimeoutEvent != null)
                TimeoutEvent(this, new EventArgs());
        }

        #endregion
    }
}
