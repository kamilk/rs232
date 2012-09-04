using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialPortCommunicator.Generics.Helpers
{
    public static class SerialPortHelper
    {
        public static void WriteIgnoringTimeout(this SerialPort port, byte[] data, int offset, int count)
        {
            try
            {
                port.Write(data, offset, count);
            }
            catch (TimeoutException)
            { }
        }
    }
}
