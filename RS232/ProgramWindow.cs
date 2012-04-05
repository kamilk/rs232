using System;
using System.Collections.Generic;
using System.Text;
using SerialPortCommunicator;
using System.Windows.Forms;
using System.Drawing;

using SerialPortCommunicator.Helpers;
using SerialPortCommunicator.Properties;

namespace SerialPortCommunicator.GUI
{
    public class ProgramWindow
    {
        private RichTextBox TextBox { get; set; }

        public ProgramWindow(RichTextBox textBox)
        {
            TextBox = textBox;
        }

        /// <summary>
        /// method to display the data to & from the port
        /// on the screen
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="msg">Message to display</param>
        [STAThread]
        public void displayMessage(string message, MessageType type, bool addNewline = true)
        {
            TextBox.Invoke(new EventHandler(delegate
            {
                TextBox.SelectedText = string.Empty;
                TextBox.SelectionFont = new Font(TextBox.SelectionFont, FontStyle.Bold);
                TextBox.SelectionColor = type.GetFontColor();
                if (type == MessageType.Incoming)
                    TextBox.AppendText(String.Format("{0} <- ", DateTime.Now));
                if (type == MessageType.Outgoing)
                    TextBox.AppendText(String.Format("{0} -> ", DateTime.Now));
                TextBox.AppendText(message);
                if (addNewline)
                    TextBox.AppendText("\n");
                TextBox.ScrollToCaret();
            }));
        }
    }
}
