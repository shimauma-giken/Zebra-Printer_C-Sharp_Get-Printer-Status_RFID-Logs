using System;
using System.Text;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using System.Diagnostics;

namespace WForm_GetRfidLogsFromZebraPrinter01
{
    public partial class Form1 : Form
    {
        string ipAddress = "192.168.4.54";
        string zplStrings = "^XA^RFR,H^XZ";

        public Form1()
        {
            InitializeComponent();
        }

        // Set RFID Log and read Tag
        private void button1_Click(object sender, EventArgs e)
        {
            SgdOverTcp_SetRfidLogEntries(ipAddress);
            SendZplOverTcp(ipAddress);

            //SgdOverTcp(ipAddress);
            //SgdOverMultiChannelNetworkConnection(ipAddress);



        }

        // Get Log Entires
        private void button2_Click(object sender, EventArgs e)
        {
            SgdOverTcp_GetRfidLogEntries(ipAddress);
            SgdOverTcp_DelRfidLogEntries(ipAddress);


        }


        // Read RFID tags by ZPL
        private void SendZplOverTcp(string theIpAddress)
        {
            // Instantiate connection for ZPL TCP port at given address
            Connection thePrinterConn = new TcpConnection(theIpAddress, TcpConnection.DEFAULT_ZPL_TCP_PORT);

            try
            {
                // Open the connection - physical connection is established here.
                thePrinterConn.Open();

                // This example prints "This is a ZPL test." near the top of the label.
                string zplData = zplStrings;


                // Send the data to printer as a byte array.
                thePrinterConn.Write(Encoding.UTF8.GetBytes(zplData));
            }
            catch (ConnectionException e)
            {
                // Handle communications error here.
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                // Close the connection to release resources.
                thePrinterConn.Close();
            }
        }


        // Set RFID Log Entires
        public static void SgdOverTcp_SetRfidLogEntries(string ipAddress)
        {
            int port = 9100;
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                SGD.SET("rfid.log.enabled", "yes", printerConnection);
                string sgdResult = SGD.GET("rfid.log.enabled", printerConnection);
                Debug.WriteLine($"SGD rfid.log.enabled is {sgdResult}");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
        }


        // Get RFID Log Entires
        public static void SgdOverTcp_GetRfidLogEntries(string ipAddress)
        {
            int port = 9100;
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                string sgdResult = SGD.GET("rfid.log.entries", printerConnection);
                Debug.WriteLine($"RFID log is -- > {sgdResult}");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
        }

        // Clear RFID Log Entires
        public static void SgdOverTcp_DelRfidLogEntries(string ipAddress)
        {
            int port = 9100;
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                SGD.SET("rfid.log.clear", "", printerConnection);
                Debug.WriteLine($"SGD rfid.log.is cleared!");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
        }



        public static void SgdOverTcp(string ipAddress)
        {
            int port = 9100;
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                SGD.SET("print.tone", "20", printerConnection);
                string printTone = SGD.GET("print.tone", printerConnection);
                Debug.WriteLine($"SGD print.tone is {printTone}");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
        }

        private static void SgdOverMultiChannelNetworkConnection(string ipAddress)
        {
            // Create and open a connection to a Link-OS printer using both the printing and the status channel
            MultichannelTcpConnection printerConnection = new MultichannelTcpConnection(ipAddress, MultichannelTcpConnection.DEFAULT_MULTICHANNEL_PRINTING_PORT, MultichannelTcpConnection.DEFAULT_MULTICHANNEL_STATUS_PORT);
            try
            {
                printerConnection.Open();

                // Get an SGD, using the status channel
                string modelName = SGD.GET("device.product_name", printerConnection);
                Debug.WriteLine($"SGD device.product_name is {modelName}");

                // Close the connection, and re-open it using only the status channel
                printerConnection.Close();
                printerConnection.OpenStatusChannel();

                // Get an SGD, again using the status channel
                string printSpeed = SGD.GET("media.speed", printerConnection);
                Debug.WriteLine($"The print speed is {printSpeed}");

                // Close the connection, and re-open it using only the printing channel
                printerConnection.Close();
                printerConnection.OpenPrintingChannel();

                // Get an SGD, using the printing channel
                string mirrorFrequency = SGD.GET("ip.mirror.freq", printerConnection);
                Debug.WriteLine($"The mirror frequency is {mirrorFrequency}");


            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
        }

    }
}