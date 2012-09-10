using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Generic.Helpers
{
    public class ArrayHelper
    {
        public static void WriteShortToByteArray(byte[] array, short value, int offset)
        {
            array[offset] = (byte)((value << 8) & 0xFF);
            array[offset + 1] = (byte)(value & 0xFF);
        }

        public static short ReadShortFromByteArray(byte[] array, int offset)
        {
            return (short)((short)((short)array[offset] << 8) | (short)array[offset + 1]);
        }
    }
}
