using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.Generics.Helpers;
using SerialPortCommunicator.RS232.Parameters;
using SerialPortCommunicator.Generics.Transceivers.Parameters;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class Rs232Receiver : IReceiver<RS232Message>
    {
        public ConnectionParameters Parameters { get; set; }

        public Rs232Receiver(ConnectionParameters parameters)
        {
            Parameters = parameters;
        }

        public RS232Message ReceiveData(SerialPort port)
        {
            Debug.WriteLine("asdf");

            byte[] dataRead = readData(port);

            Debug.WriteLine(dataRead.Length);
            try
            {
                return new RS232Message(dataRead);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new MessageException("Nie znaleziono znacznika końca");
            }
        }

        private byte[] readData(SerialPort port)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                while (port.BytesToRead > 0)
                {
                    int bytes = port.BytesToRead;
                    Debug.WriteLine(port.BytesToRead);
                    byte[] response = new byte[bytes];
                    port.Read(response, 0, bytes);
                    writer.Write(response);
                }

                return stream.ToArray();
            }
        }
    }
}
