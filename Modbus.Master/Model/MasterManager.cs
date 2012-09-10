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
            byte[] data;

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(register);
                writer.Write(value);
                writer.Flush();
                data = stream.ToArray();
            }

            modbusManager.SendMessage(new ModbusMessage(data, slaveAddress, 0x03));
        }

        private void OnMessageReceived(object sender, DataReceivedEventArgs<ModbusMessage> e)
        {
            Debug.WriteLine("Unimplemented MasterManager.OnMessageReceived() method called.");
        }
    }
}
