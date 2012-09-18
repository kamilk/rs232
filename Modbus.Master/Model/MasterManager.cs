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
        private Dictionary<int, ModbusRequest> _sentRequests = new Dictionary<int, ModbusRequest>();

        public event EventHandler<ModbusDataReadEventArgs> DataReadEvent;
        public event EventHandler<RequestTimeoutEventArgs> RequestTimeoutEvent;
        public event EventHandler<ModbusEventArgs> RequestFailedEvent;

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
                ModbusRequest request = AddSentRequest(slaveAddress, register, message, OnResponseToWriteRequestReceived);

                _modbusManager.SendMessage(message);

                request.StartTimer();
            }
        }

        public void BeginReadFromSlave(byte slaveAddress, short register, Action<string> onSuccessCallback)
        {
            if (_sentRequests.ContainsKey(slaveAddress))
                return;

            var data = new byte[2];
            ArrayHelper.WriteShortToByteArray(data, register, 0);
            ModbusMessage message = new ModbusMessage(data, slaveAddress, ReadFunctionCode);

            ModbusRequest request = AddSentRequest(slaveAddress, register, message, OnResponseToReadRequestReceived);
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
                request.ResponseHandler(request, e.Message);
                request.TimeoutEvent -= OnRequestTimeout;
                _sentRequests.Remove(slaveAddress);
            }
        }

        private void OnRequestTimeout(object sender, EventArgs e)
        {
            var request = (ModbusRequest)sender;
            if (request.AttemptsLeft > 0)
            {
                if (RequestTimeoutEvent != null)
                {
                    RequestTimeoutEvent(this, new RequestTimeoutEventArgs(
                        request.SlaveAddress, request.RegisterNumber, request.AttemptsLeft));
                }

                request.DecrementAttemptsLeft();
                _modbusManager.SendMessage(request.Message);
                request.StartTimer();
            }
            else
            {
                _sentRequests.Remove(request.SlaveAddress);
                if (RequestFailedEvent != null)
                    RequestFailedEvent(this, new ModbusEventArgs(request.SlaveAddress, request.RegisterNumber));
            }
        }

        private void OnResponseToReadRequestReceived(ModbusRequest request, ModbusMessage message)
        {
            if (message.Function == ReadFunctionCode)
            {
                string readValue = new UTF8Encoding().GetString(message.Data);

                if (DataReadEvent != null)
                    DataReadEvent(this, new ModbusDataReadEventArgs(request.SlaveAddress, request.RegisterNumber, readValue));
            }
        }

        private void OnResponseToWriteRequestReceived(ModbusRequest request, ModbusMessage message)
        { }

        private ModbusRequest AddSentRequest(byte slaveAddress, short register, ModbusMessage message, Action<ModbusRequest, ModbusMessage> ResponseHandler)
        {
            ModbusRequest request = new ModbusRequest(message, slaveAddress, register);
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
