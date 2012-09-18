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

        private ModbusCommunicationManager _modbusManager;
        private List<ReadRequestData> _readRequests = new List<ReadRequestData>();
        private Dictionary<int, ModbusRequest> _sentRequests = new Dictionary<int, ModbusRequest>();

        public bool IsPortOpen
        {
            get { return _modbusManager.IsPortOpen; }
        }

        public MasterManager()
        {
            _modbusManager = new ModbusCommunicationManager();
            _modbusManager.MessageReceived += OnMessageReceived;
        }

        public void OpenPort(ModbusConnectionParameters parameters)
        {
            _modbusManager.OpenPort(parameters);
        }

        public void ClosePort()
        {
            _modbusManager.ClosePort();
        }

        public void Dispose()
        {
            ClosePort();
        }

        public void WriteToSlave(byte slaveAddress, short register, string value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(register);
                writer.Write(new UTF8Encoding().GetBytes(value));
                writer.Flush();
                byte[] data = stream.ToArray();

                ModbusMessage message = new ModbusMessage(data, slaveAddress, WriteFunctionCode);
                ModbusRequest request = AddSentRequest(slaveAddress, message, OnResponseToWriteRequestReceived);

                _modbusManager.SendMessage(message);

                request.StartTimer();
            }
        }

        public void BeginReadFromSlave(byte slaveAddress, short register, Action<string> onSuccessCallback)
        {
            if (GetReadRequestData(slaveAddress) != null)
                return;

            _readRequests.Add(new ReadRequestData()
            {
                Address = slaveAddress,
                Register = register,
                OnSuccessCallback = onSuccessCallback
            });

            var data = new byte[2];
            ArrayHelper.WriteShortToByteArray(data, register, 0);
            ModbusMessage message = new ModbusMessage(data, slaveAddress, ReadFunctionCode);

            ModbusRequest request = AddSentRequest(slaveAddress, message, OnResponseToReadRequestReceived);
            _modbusManager.SendMessage(message);
            request.StartTimer();
        }

        private void OnMessageReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            int slaveAddress = e.Message.Address;
            ModbusRequest request;
            if (_sentRequests.TryGetValue(slaveAddress, out request))
            {
                request.StopTimer();
                request.ResponseHandler(e.Message);
                request.TimeoutEvent -= OnRequestTimeout;
                _sentRequests.Remove(slaveAddress);
            }
        }

        private ReadRequestData? GetReadRequestData(byte slaveAddress)
        {
            return _readRequests.Where(request => request.Address == slaveAddress).Cast<ReadRequestData?>()
                .FirstOrDefault();
        }

        void OnRequestTimeout(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void OnResponseToReadRequestReceived(ModbusMessage message)
        {
            if (message.Function == ReadFunctionCode)
            {
                ReadRequestData? readRequest = GetReadRequestData(message.Address);
                if (readRequest != null)
                {
                    string readValue = new UTF8Encoding().GetString(message.Data);
                    readRequest.Value.OnSuccessCallback(readValue);
                    _readRequests.Remove(readRequest.Value);
                }
            }
        }

        void OnResponseToWriteRequestReceived(ModbusMessage message)
        { }

        private ModbusRequest AddSentRequest(byte slaveAddress, ModbusMessage message, Action<ModbusMessage> ResponseHandler)
        {
            ModbusRequest request = new ModbusRequest(message, slaveAddress);
            request.TimeoutEvent += new EventHandler(OnRequestTimeout);
            request.ResponseHandler = ResponseHandler;
            _sentRequests.Add(slaveAddress, request);
            return request;
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
