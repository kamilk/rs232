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

        public IEnumerable<RS232Message> ReceiveMessages(SerialPort port)
        {
            byte[] data = readData(port);

            Debug.WriteLine(data.Length);
            return SplitIntoMessages(data);
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

        private IEnumerable<RS232Message> SplitIntoMessages(byte[] data)
        {
            if (Parameters.EndMarker == EndMarker.NONE)
                return new RS232Message[] { new RS232Message(data) };

            var messages = new List<RS232Message>();

            byte expectedByte = Parameters.EndMarker == EndMarker.LF ? (byte)'\n' : (byte)'\r';
            int delimiterLength = Parameters.EndMarker == EndMarker.CRLF ? 2 : 1;
            int messageStart = 0;

            int i = 0;
            while (i <= data.Length - delimiterLength)
            {
                if (data[i] == expectedByte && (Parameters.EndMarker != EndMarker.CRLF || IsNextByteLf(data, i)))
                {
                    if (messageStart == i)
                    {
                        i += delimiterLength;
                        messageStart = i;
                        continue;
                    }

                    messages.Add(CreateMessageFromSegmentOfArray(data, messageStart, i - messageStart));

                    messageStart = i + delimiterLength;

                    i += delimiterLength;
                }
                else
                    i++;
            }

            if (messageStart != data.Length)
                messages.Add(CreateMessageFromSegmentOfArray(data, messageStart, data.Length - messageStart));

            return messages;
        }

        private bool IsNextByteLf(byte[] data, int i)
        {
            return i < data.Length - 1 && data[i + 1] == (byte)'\n';
        }

        private static RS232Message CreateMessageFromSegmentOfArray(byte[] data, int messageStart, int messageLength)
        {
            var messageData = new byte[messageLength];
            Array.Copy(data, messageStart, messageData, 0, messageLength);
            RS232Message newRS232Message = new RS232Message(messageData);
            return newRS232Message;
        }
    }
}
