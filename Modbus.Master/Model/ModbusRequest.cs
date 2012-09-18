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
        public int _timeout;

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

        public ModbusRequest(ModbusMessage message, byte slaveAddress, short register, int attempts = 3, int timeout = 5000)
        {
            Message = message;
            SlaveAddress = slaveAddress;
            RegisterNumber = register;
            AttemptsLeft = attempts - 1;
            _timeout = timeout;
        }

        #endregion

        #region Methods

        public void StartTimer()
        {
            StopTimer();
            _timer = new Timer(OnTimeout, null, _timeout, Timeout.Infinite);
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
