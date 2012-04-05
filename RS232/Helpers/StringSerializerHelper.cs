using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SerialPortCommunicator.Transceivers;

namespace SerialPortCommunicator.Helpers
{
    public static class StringSerializerHelper
    {
        public static byte[] SerializedString(this string text)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, text);
            ms.Seek(0, 0);

            return ms.ToArray();
        }

        public static string DeserializedString(this byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);
            try
            {
                return (string) bf.Deserialize(ms);
            }
            catch(System.Runtime.Serialization.SerializationException ex)
            {
                throw new MessageException("Nie udało się odczytać wiadomości");
            }
        }
    }
}
