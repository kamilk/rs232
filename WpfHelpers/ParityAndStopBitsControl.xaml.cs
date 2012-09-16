using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SerialPortCommunicator.Modbus;
using System.ComponentModel;

namespace SerialPortCommunicator.Modbus.CommonView
{
    /// <summary>
    /// Interaction logic for ParityAndStopBitsControl.xaml
    /// </summary>
    public partial class ParityAndStopBitsControl : UserControl
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(ModbusParityAndStopBits), typeof(ParityAndStopBitsControl));

        public ModbusParityAndStopBits Value
        {
            get { return (ModbusParityAndStopBits)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public ParityAndStopBitsControl()
        {
            InitializeComponent();
        }

        private void e1RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Value = ModbusParityAndStopBits.E1;
        }

        private void o1RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            Value = ModbusParityAndStopBits.O1;
        }

        private void n2RadioButton3_Checked(object sender, RoutedEventArgs e)
        {
            Value = ModbusParityAndStopBits.N2;
        }
    }
}
