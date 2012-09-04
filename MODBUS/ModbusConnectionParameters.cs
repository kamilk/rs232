using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SerialPortCommunicator.Generic.Properties;
using SerialPortCommunicator.Generic.Parameters;
using SerialPortCommunicator.Modbus.MessageProcessors;

namespace SerialPortCommunicator.Modbus
{
    struct ModbusConnectionParameters
    {
        public ModbusMode Mode;
        public ModbusParityAndStopBits ParityAndStopBits;
        public string PortName;
        public int BaudRate;
        public Handshake Handshake;

        internal ConnectionParameters GetConnectionParameters()
        {
            int dataBits;
            Parity parity;
            StopBits stopBits;
            EndMarker endMarker;

            switch (Mode)
            {
                case ModbusMode.Ascii:
                    dataBits = 7;
                    endMarker = EndMarker.CRLF;
                    break;
                case ModbusMode.Rtu:
                    dataBits = 8;
                    endMarker = EndMarker.NONE;
                    break;
                default:
                    throw new Exception("Unknown mode");
            }

            switch (ParityAndStopBits)
            {
                case ModbusParityAndStopBits.E1:
                    parity = Parity.Even;
                    stopBits = StopBits.One;
                    break;
                case ModbusParityAndStopBits.O1:
                    parity = Parity.Odd;
                    stopBits = StopBits.One;
                    break;
                case ModbusParityAndStopBits.N2:
                    parity = Parity.None;
                    stopBits = StopBits.Two;
                    break;
                default:
                    throw new Exception("Unknown ParityAndStopBits");
            }

            return new ConnectionParameters(
                PortName,
                BaudRate,
                dataBits,
                parity,
                Handshake,
                stopBits,
                endMarker);
        }

        internal IModbusMessageProcessor GetMessageProcessor()
        {
            switch (Mode)
            {
                case ModbusMode.Ascii:
                    return new AsciiMessageProcessor();
                case ModbusMode.Rtu:
                    return new RtuMessageProcessor();
                default:
                    throw new Exception("Unsupported value of Mode");
            }
        }
    }
}
