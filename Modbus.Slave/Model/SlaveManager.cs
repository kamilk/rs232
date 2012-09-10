using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics;
using SerialPortCommunicator.Generic.Helpers;

namespace SerialPortCommunicator.Modbus.Slave.Model
{
    class SlaveManager : IDisposable
    {
        #region Constants

        private const int NumberOfRegisters = 5;
        private const int WriteFunctionCode = 0x06;
        private const int ReadFunctionCode = 0x03;

        #endregion

        #region Properties

        public byte Address { get; set; }

        #endregion

        #region Fields

        private ModbusCommunicationManager modbusManager 
            = new ModbusCommunicationManager();

        private short[] registers;

        #endregion

        #region Constructors

        public SlaveManager()
        {
            modbusManager.MessageReceived += OnMessageReceived;

            registers = new short[NumberOfRegisters];
            for (int i = 0; i < registers.Length; i++)
                registers[i] = 0;

            Address = 1;
        }

        #endregion

        #region Methods

        public void OpenPort(ModbusConnectionParameters parameters)
        {
            modbusManager.OpenPort(parameters);
        }

        public void ClosePort()
        {
            modbusManager.ClosePort();
        }

        public void Dispose()
        {
            ClosePort();
        }

        private void OnMessageReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            if (e.Message.Address != Address)
                return;

            if (e.Message.Function == ReadFunctionCode && e.Message.Data.Length >= 4)
            {
                short register = ArrayHelper.ReadShortFromByteArray(e.Message.Data, 0);

                var responseData = new byte[3];
                responseData[0] = 2;
                ArrayHelper.WriteShortToByteArray(responseData, registers[register], 1);
                modbusManager.SendMessage(new ModbusMessage(responseData, Address, ReadFunctionCode));
            }
            else if (e.Message.Function == WriteFunctionCode && e.Message.Data.Length >= 4)
            {
                short register = ArrayHelper.ReadShortFromByteArray(e.Message.Data, 0);
                short value = ArrayHelper.ReadShortFromByteArray(e.Message.Data, 2);
                registers[register] = value;
                modbusManager.SendMessage(e.Message);
            }
        }

        #endregion

        #region Static fields and properties

        private static SlaveManager _instance;
        public static SlaveManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SlaveManager();
                return _instance;
            }
        }

        #endregion
    }
}
