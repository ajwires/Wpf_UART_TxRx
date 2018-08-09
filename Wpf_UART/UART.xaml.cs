using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Threading;
using System.Threading;

namespace Wpf_UART
{
    /// <summary>
    /// Interaction logic for UART.xaml
    /// </summary>
    public partial class UART : Page
    {
        private SerialPort serial;

        public UART(SerialPort rxPort)
        {
            InitializeComponent();
            serial = rxPort;
            //set serial recieve function
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
            serial.Open();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            //get data from text box
            String data = uartTxTextBox.Text.ToString();
            Send(data);
        }
        //send serial data
        private void Send(String data)
        {
            if (serial.IsOpen)
            {
                try
                {
                    // Send the binary data out the port
                    byte[] hexstring = Encoding.ASCII.GetBytes(data);
                    foreach (byte hexval in hexstring)
                    {
                        byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                                    // to byte[] to write
                        serial.Write(_hexval, 0, 1);
                        Thread.Sleep(1);
                    }
                    txListBox.Items.Add(data);  //add to data sent.
                }
                catch (Exception ex)
                {
                    /*para.Inlines.Add("Failed to SEND" + data + "\n" + ex + "\n");
                    mcFlowDoc.Blocks.Add(para);
                    Commdata.Document = mcFlowDoc;*/
                }
            }
        }
        //recieve data thread
        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // Collecting the characters received to our 'buffer' (string).
            var recieved_data = serial.ReadExisting();
            //dispatcher to the UI thread
            Dispatcher.Invoke(() =>
            { 
                // update text box
                rxListBox.Items.Add(recieved_data);
            });

        }
    }
}
