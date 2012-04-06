﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SerialPortCommunicator.Properties;
using RS232.Parameters;
using System.Diagnostics;

namespace RS232.Transceivers
{
    public class Transmitter : ITransmitter
    {
        public ConnectionParameters Parameters { get; set; }
        private bool ConnectionXON { get; set; }

        public Transmitter(ConnectionParameters parameters)
        {
            Parameters = parameters;
            ConnectionXON = true;
        }

        public void TransmitData(SerialPort port, byte[] message)
        {
            port.DiscardOutBuffer();

            int messageWriteOffset = 0;
            message = appendEndMarker(message);

            int bytesLeft = message.Length;
            Debug.Write(String.Format("> Do wysłania: {0} <\n", bytesLeft));

            while (bytesLeft > 0)
            {
                if (isXON(port) && (port.BytesToWrite < port.WriteBufferSize))
                {
                    if (port.WriteBufferSize - port.BytesToWrite < bytesLeft)
                    {
                        port.Write(message, messageWriteOffset, port.WriteBufferSize - port.BytesToWrite);
                        bytesLeft -= port.WriteBufferSize - port.BytesToWrite;
                        Debug.Write("> Wysył: ");
                        Debug.Write(port.WriteBufferSize - port.BytesToWrite);
                        Debug.Write(" <\n");
                    }
                    else
                    {
                        port.Write(message, messageWriteOffset, bytesLeft);
                        Debug.Write("> wysył: ");
                        Debug.Write(bytesLeft);
                        Debug.Write(" <\n");
                        bytesLeft = 0;
                    }
                }
                else
                {
                    Debug.Write("> XOFF <\n");
                } 
            }
        }

        private byte[] appendEndMarker(byte[] message)
        {
            byte[] ending = new byte[0];
            switch (Parameters.EndMarker)
            {
                case (EndMarker.CR):
                    ending = new byte[] { 13 };
                    break;
                case (EndMarker.CRLF):
                    ending = new byte[] { 13, 10 };
                    break;
                case (EndMarker.LF):
                    ending = new byte[] { 10 };
                    break;
                default:
                    break;
            }
            byte[] result = new byte[message.Length + ending.Length];
            Array.Copy(message, result, message.Length);
            Array.Copy(ending, 0, result, message.Length, ending.Length);
            return result;
        }

        private bool isXON(SerialPort port)
        {
            switch (Parameters.XONType)
            {
                case (XONType.DTRDSR):
                    return port.DsrHolding;
                case (XONType.RTSCTS):
                    return port.CtsHolding;
                case (XONType.PROGRAM):
                    return ConnectionXON;
                default:
                    return false;
            }
        }
        
        public void SetXOFF()
        {
            ConnectionXON = false;
        }

        public void SetXON()
        {
            ConnectionXON = true;
        }
    }
}