using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Generic.Helpers
{
    public static class HexHelper
    {
        public static byte[] EncodeByteAsHex(byte value)
        {
            string valueString = EncodeByteAsHexString(value);
            return new ASCIIEncoding().GetBytes(valueString);
        }

        public static string EncodeByteAsHexString(byte value)
        {
            return string.Format(
                            "{0}{1:X}",
                            value < 16 ? "0" : "",
                            value);
        }

        public static byte[] DecodeHexArray(byte[] hexArray)
        {
            return DecodeHexArray(hexArray, 0, hexArray.Length);
        }

        public static byte[] DecodeHexArray(byte[] hexArray, int offset, int count)
        {
            var result = new byte[count / 2];

            for (int i = 0; i < count / 2; i++)
            {
                byte convertToByte = DecodeHex(hexArray[offset + i * 2], hexArray[offset + i * 2 + 1]);
                result[i] = convertToByte;
            }

            return result;
        }

        public static byte DecodeHex(byte sixteensDigitCharacter, byte unitsDigitCharacter)
        {
            var hexBytes = new byte[] { sixteensDigitCharacter, unitsDigitCharacter };
            string hex = new ASCIIEncoding().GetString(hexBytes);
            return Convert.ToByte(hex, 16);
        }

        public static byte[] DecodeHexString(string messageHexString)
        {
            return HexHelper.DecodeHexArray(new ASCIIEncoding().GetBytes(messageHexString.Replace(" ", "")));
        }

        public static string EncodeAsHexString(byte[] data)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                builder.Append(EncodeByteAsHexString(data[i]));
            return builder.ToString();
        }

        public static byte[] EncodeAsHexArray(byte[] data)
        {
            return new ASCIIEncoding().GetBytes(EncodeAsHexString(data));
        }
    }
}
