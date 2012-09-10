using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics;
using System.Diagnostics;
using System.IO;
using SerialPortCommunicator.Generic.Helpers;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class MasterManager : IDisposable
    {
        private const int WriteFunctionCode = 0x06;
        private const int ReadFunctionCode = 0x03;

        private ModbusCommunicationManager modbusManager;
        private List<ReadRequestData> readRequests = new List<ReadRequestData>();

        public bool IsPortOpen
        {
            get { return modbusManager.IsPortOpen; }
        }

        public MasterManager()
        {
            modbusManager = new ModbusCommunicationManager();
            modbusManager.MessageReceived += OnMessageReceived;
        }

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

        public void WriteToSlave(byte slaveAddress, short register, short value)
        {
            var data = new byte[4];

            ArrayHelper.WriteShortToByteArray(data, register, 0);
            ArrayHelper.WriteShortToByteArray(data, value, 2);

            modbusManager.SendMessage(new ModbusMessage(data, slaveAddress, WriteFunctionCode));
        }

        public void BeginReadFromSlave(byte slaveAddress, short register, Action<short> onSuccessCallback)
        {
            if (GetReadRequestData(slaveAddress) != null)
                return;

            readRequests.Add(new ReadRequestData() 
            { 
                Address = slaveAddress, 
                Register = register, 
                OnSuccessCallback = onSuccessCallback 
            });

            var data = new byte[4];
            ArrayHelper.WriteShortToByteArray(data, register, 0);
            ArrayHelper.WriteShortToByteArray(data, 1, 2);
            modbusManager.SendMessage(new ModbusMessage(data, slaveAddress, ReadFunctionCode));
        }

        private void OnMessageReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            ModbusMessage message = e.Message;
            if (message.Function == ReadFunctionCode && message.Data.Length >= 3)
            {
                ReadRequestData? readRequest = GetReadRequestData(message.Address);
                if (readRequest != null)
                {
                    short readValue = ArrayHelper.ReadShortFromByteArray(message.Data, 1);
                    readRequest.Value.OnSuccessCallback(readValue);
                    readRequests.Remove(readRequest.Value);
                }
            }
        }

        private ReadRequestData? GetReadRequestData(byte slaveAddress)
        {
            return readRequests.Where(request => request.Address == slaveAddress).Cast<ReadRequestData?>()
                .FirstOrDefault();
        }

        private static MasterManager _instance;
        public static MasterManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MasterManager();
                return _instance;
            }
        }
    }
}
