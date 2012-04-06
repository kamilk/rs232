using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SerialPortCommunicator.Properties;
using System.IO;
using RS232.Parameters;
using RS232.Helpers;
using System.Diagnostics;

namespace RS232.Transceivers
{
    public class Receiver : IReceiver
    {
        public ConnectionParameters Parameters { get; set; }
        protected bool TransmissionEnded { get; set; }
        protected bool PreviousCanAcceptData { get; set; }
        protected MemoryStream MemoryStream { get; set; }
        protected BinaryWriter ResponseWriter { get; set; }

        public Receiver(ConnectionParameters parameters)
        {
            Parameters = parameters;
            PreviousCanAcceptData = true;
            foo = true;
        }

        public byte[] ReceiveData(SerialPort port)
        {
            Debug.WriteLine("hello");
            MemoryStream = new MemoryStream();
            ResponseWriter = new BinaryWriter(MemoryStream);

            TransmissionEnded = false;

            while (!TransmissionEnded)
            {
                if (canAcceptData(port))
                {
                    sendXONIfNecessary(port);
                }
                else
                {
                    sendXOFFIfNecessary(port);
                }
                readData(port);
            }

            return MemoryStream.ToArray();
        }

        private bool canAcceptData(SerialPort port)
        {
            Debug.Write(String.Format("< Do odbioru: {0} >\n", port.BytesToRead));
            return port.BytesToRead == 0;
        }

        private void readData(SerialPort port)
        {
            if (port.BytesToRead > 0)
            {
                int bytes = port.BytesToRead;
                byte[] response = new byte[bytes];
                port.Read(response, 0, bytes);
                Debug.Write(String.Format("< Odbiór: {0} >\n", bytes));
                ResponseWriter.Write(response);
                if (hasEndMarker(response))
                    TransmissionEnded = true;
                if (hasXOFFCharacter(response))
                    throw new XOFFReceivedException();
                if (hasXONCharacter(response))
                    throw new XONReceivedException();
            }
        }

        private bool hasXONCharacter(byte[] response)
        {
            return Parameters.XONType == XONType.PROGRAM && response.ToList().Contains((byte)ControlChars.XOFF);
        }

        private bool hasXOFFCharacter(byte[] response)
        {
            return Parameters.XONType == XONType.PROGRAM && response.ToList().Contains((byte)ControlChars.XON);
        }

        private void sendXOFFIfNecessary(SerialPort port)
        {
            if (PreviousCanAcceptData == true)
                sendXOFF(port);
            PreviousCanAcceptData = false;
        }

        private void sendXONIfNecessary(SerialPort port)
        {
            if (PreviousCanAcceptData == false)
                sendXON(port);
            PreviousCanAcceptData = true;
        }

        private void sendXON(SerialPort port)
        {
            Debug.Write("< Włączam >\n");
            switch (Parameters.XONType)
            {
                case (XONType.DTRDSR):
                    port.DtrEnable = true;
                    break;
                case (XONType.RTSCTS):
                    port.RtsEnable = true;
                    break;
                case (XONType.PROGRAM):
                    port.Write(new byte[] { (byte)ControlChars.XON }, 0, 1);
                    break;
            }
        }

        private void sendXOFF(SerialPort port)
        {
            Debug.Write("< Wyłączam >\n");
            switch (Parameters.XONType)
            {
                case (XONType.DTRDSR):
                    port.DtrEnable = false;
                    break;
                case (XONType.RTSCTS):
                    port.RtsEnable = false;
                    break;
                case (XONType.PROGRAM):
                    port.Write(new byte[] { (byte)ControlChars.XOFF }, 0, 1);
                    break;
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

        public bool foo { get; set; }
    }
}
