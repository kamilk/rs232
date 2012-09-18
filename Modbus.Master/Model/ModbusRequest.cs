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
        public int AttemptsLeft { get; private set; }

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
            AttemptsLeft = 3;
        }

        #endregion

        #region Methods

        public void StartTimer()
        {
            StopTimer();
            _timer = new Timer(OnTimeout, null, 5000, Timeout.Infinite);
        }

        public void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        public void DecrementAttemptsLeft()
        {
            if (AttemptsLeft > 0)
                AttemptsLeft--;
        }

        private void OnTimeout(object state)
        {
            StopTimer();
            if (TimeoutEvent != null)
                TimeoutEvent(this, new EventArgs());
        }

        #endregion
    }
}
