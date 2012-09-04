using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SerialPortCommunicator.Generics.Transceivers;
using System.Text;

namespace SerialPortCommunicator.Generics.Helpers
{
    public static class StringSerializerHelper
    {
        public static byte[] SerializedString(this string text)
        {
            return new ASCIIEncoding().GetBytes(text);
        }

        public static string DeserializedString(this byte[] data)
        {
            return new ASCIIEncoding().GetString(data);
        }
    }
}
