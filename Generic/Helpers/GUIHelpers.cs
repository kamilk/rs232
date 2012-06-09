using System.Drawing;
using SerialPortCommunicator.Generics.Properties;
namespace SerialPortCommunicator.Helpers
{
    public static class GUIHelpers
    {
        public static Color GetFontColor(this MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Incoming:
                    return Color.Blue;
                case MessageType.Outgoing:
                    return Color.Green;
                case MessageType.Normal:
                    return Color.Black;
                case MessageType.Warning:
                    return Color.Orange;
                case MessageType.Error:
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }
    }

}