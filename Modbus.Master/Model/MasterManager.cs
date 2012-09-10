using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialPortCommunicator.Generics;
using System.Diagnostics;
using System.IO;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    class MasterManager : IDisposable
    {
        private ModbusCommunicationManager modbusManager;

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

            data[0] = (byte)((register << 8) & 0xFF);
            data[1] = (byte)(register & 0xFF);
            data[2] = (byte)((value << 8) & 0xFF);
            data[3] = (byte)(value & 0xFF);

            modbusManager.SendMessage(new ModbusMessage(data, slaveAddress, 0x06));
        }

        private void OnMessageReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            Debug.WriteLine("Unimplemented MasterManager.OnMessageReceived() method called.");
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
