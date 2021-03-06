﻿using System;
using System.Drawing;
using System.Windows.Forms;
using SerialPortCommunicator.Generics;
using SerialPortCommunicator.Generics.Helpers;

namespace SerialPortCommunicator.Generics.Helpers
{
    public static class RichTextBoxHelper
    {
        /// <summary>
        /// method to display the data to & from the port
        /// on the screen
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="msg">Message to display</param>
        public static void InvokeDisplayMessage(this RichTextBox TextBox, string message, MessageType type, bool addNewline = true)
        {
            TextBox.Invoke(new Action(() =>
            {
                TextBox.SelectedText = string.Empty;
                TextBox.SelectionFont = new Font(TextBox.SelectionFont, FontStyle.Bold);
                TextBox.SelectionColor = type.GetFontColor();
                if (type == MessageType.Incoming)
                    TextBox.AppendText(String.Format("{0} <- ", DateTime.Now));
                if (type == MessageType.Outgoing)
                    TextBox.AppendText(String.Format("{0} -> ", DateTime.Now));
                if (type == MessageType.Error)
                    TextBox.AppendText(String.Format("{0}    ", DateTime.Now));
                TextBox.AppendText(message);
                if (addNewline)
                    TextBox.AppendText("\n");
                TextBox.ScrollToCaret();
            }));
        }
    }
}
