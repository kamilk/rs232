using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics;
using SerialPortCommunicator.Generic.Helpers;
using System.IO;

namespace SerialPortCommunicator.Modbus.Slave.Model
{
    class SlaveManager : IDisposable
    {
        #region Constants

        public const int NumberOfRegisters = 5;
        private const int WriteFunctionCode = 0x06;
        private const int ReadFunctionCode = 0x03;

        #endregion

        #region Properties

        public byte Address { get; set; }

        #endregion

        #region Events

        public event EventHandler<RegisterValueChangedEventArgs> RegisterValueChanged;

        #endregion

        #region Fields

        private ModbusCommunicationManager modbusManager 
            = new ModbusCommunicationManager();

        private string[] registers;

        #endregion

        #region Constructors

        public SlaveManager()
        {
            modbusManager.MessageReceived += OnMessageReceived;

            registers = new string[NumberOfRegisters];
            for (int i = 0; i < registers.Length; i++)
                registers[i] = string.Empty;

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

        public string GetRegisterValue(short registerNumber)
        {
            return registers[registerNumber];
        }

        public void SetRegisterValue(short registerNumber, string registerValue)
        {
            registers[registerNumber] = registerValue;
            NotifyRegisterValueChanged(registerNumber, registerValue);
        }

        private void OnMessageReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            ModbusMessage message = e.Message;
            if (message.Address != Address && !message.IsBroadcast)
                return;

            if (message.Function == ReadFunctionCode && !message.IsBroadcast)
                OnReadRequestReceived(message);
            else if (message.Function == WriteFunctionCode)
                OnWriteRequestReceived(message);
        }

        private void OnReadRequestReceived(ModbusMessage message)
        {
            if (message.Data.Length < 2)
                return;

            short register = ArrayHelper.ReadShortFromByteArray(message.Data, 0);

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(new UTF8Encoding().GetBytes(GetRegisterValue(register)));
                writer.Flush();
                modbusManager.SendMessage(new ModbusMessage(stream.ToArray(), Address, ReadFunctionCode));
            }
        }


        private void OnWriteRequestReceived(ModbusMessage message)
        {
            if (message.Data.Length < 4)
                return;

            using (var stream = new MemoryStream(message.Data))
            using (var reader = new BinaryReader(stream))
            {
                var register = reader.ReadInt16();
                var data = new byte[message.Data.Length - 2];
                reader.Read(data, 0, message.Data.Length - 2);
                string newValue = new UTF8Encoding().GetString(data);
                SetRegisterValue(register, newValue);
            }

            modbusManager.SendMessage(message);
        }

        private void NotifyRegisterValueChanged(short register, string value)
        {
            if (RegisterValueChanged != null)
                RegisterValueChanged(this, new RegisterValueChangedEventArgs(register, value));
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
