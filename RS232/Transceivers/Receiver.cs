using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using SerialPortCommunicator.Generics.Transceivers;
using SerialPortCommunicator.RS232.Helpers;
using SerialPortCommunicator.RS232.Parameters;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generic.Parameters;

namespace SerialPortCommunicator.RS232.Transceivers
{
    public class Receiver : IReceiver
    {
        public ConnectionParameters Parameters { get; set; }
        protected bool TransmissionEnded { get; set; }
        protected MemoryStream MemoryStream { get; set; }
        protected BinaryWriter ResponseWriter { get; set; }

        public Receiver(ConnectionParameters parameters)
        {
            Parameters = parameters;
        }

        public byte[] ReceiveData(SerialPort port)
        {
            MemoryStream = new MemoryStream();
            ResponseWriter = new BinaryWriter(MemoryStream);

            TransmissionEnded = false;

            Debug.WriteLine("asdf");

            while (!TransmissionEnded)
            {
                readData(port);
            }

            Debug.WriteLine(MemoryStream.ToArray().Length);
            try
            {
                return responseWithoutEndMarker(); ;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new MessageException("Nie znaleziono znacznika końca");
            }
        }

        private byte[] responseWithoutEndMarker()
        {
            byte[] response = MemoryStream.ToArray();
            List<byte> listResponse = response.ToList();
            switch (Parameters.EndMarker)
            {
                case (EndMarker.CR):
                    return listResponse.GetRange(0, listResponse.IndexOf((byte)ControlChars.CR)).ToArray();
                case (EndMarker.LF):
                    return listResponse.GetRange(0, listResponse.IndexOf((byte)ControlChars.LF)).ToArray();
                case (EndMarker.CRLF):
                    return listResponse.GetRange(0, listResponse.SubListIndex(0, (new byte[] { (byte)ControlChars.CR, (byte)ControlChars.LF }).ToList())).ToArray();
                default:
                    return response;
            }
        }

        private void readData(SerialPort port)
        {
            if (port.BytesToRead > 0)
            {
                int bytes = port.BytesToRead;
                Debug.WriteLine(port.BytesToRead);
                byte[] response = new byte[bytes];
                port.Read(response, 0, bytes);
                ResponseWriter.Write(response);
                if (hasEndMarker(response))
                    TransmissionEnded = true;
            }
        }

        private bool hasEndMarker(byte[] response)
        {
            switch (Parameters.EndMarker)
            {
                case (EndMarker.CR):
                    return response.ToList().Contains((byte)ControlChars.CR);
                case (EndMarker.LF):
                    return response.ToList().Contains((byte)ControlChars.LF);
                case (EndMarker.CRLF):
                    return response.ToList().Intersect((new byte[] { (byte)ControlChars.CR, (byte)ControlChars.LF }).ToList()).Any();
                default:
                    return true;
            }
        }

    }
}
