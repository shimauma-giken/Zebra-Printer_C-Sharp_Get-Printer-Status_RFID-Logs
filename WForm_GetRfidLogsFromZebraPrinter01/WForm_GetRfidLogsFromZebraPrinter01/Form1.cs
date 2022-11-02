using System;
using System.Text;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using System.Diagnostics;
using System.Threading;

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
            SendZplOverTcp(ipAddress, zplStrings);

            //SgdOverTcp(ipAddress);
            //SgdOverMultiChannelNetworkConnection(ipAddress);



        }


        // Get RFID Log Entires
        private void button2_Click(object sender, EventArgs e)
        {
            SgdOverTcp_GetRfidLogEntries(ipAddress);
            SgdOverTcp_DelRfidLogEntries(ipAddress);


        }

        // Encode 10 tags and get encode results.
        private void button3_Click(object sender, EventArgs e)
        {
            string str;
            string t = ",00000000,";

            Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Delete set rfid log.");
            SgdOverTcp_DelRfidLogEntries(ipAddress);


            // Create Loop x 10

            for (int i = 0; i< 10; i++)
            {

                /*
                // ZPL送信後に実施した方がパフォーマンスが良いため、移動。 
                //
                // Get Printer Status 
                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Start getting printer status.");
                while (true)
                {
                    Thread.Sleep(100);
                    str = SgdOverTcp_GetPrinterStatus(ipAddress);
                    //Debug.WriteLine($"Length is -- > {str.IndexOf(t)}");
                    if (str != null)
                    {
                        Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Recieved printer status.");
                        Debug.WriteLine($"Printer Status is -- > {str}");

                        // Write Error check code Here!

                        break;
                    }
                }
                */



                zplStrings = "^XA^RFW,H^FD00000000000" + i.ToString() + "^XZ";
                Debug.WriteLine(zplStrings);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Label #" + i);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Set rfid log.");
                SgdOverTcp_SetRfidLogEntries(ipAddress);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Send ZPL.");
                SendZplOverTcp(ipAddress, zplStrings);

                // Get Printer Status 
                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Start getting printer status.");
                while (true)
                {
                    Thread.Sleep(100);
                    str = SgdOverTcp_GetPrinterStatus(ipAddress);
                    //Debug.WriteLine($"Length is -- > {str.IndexOf(t)}");
                    if (str != null)
                    {
                        Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Recieved printer status.");
                        Debug.WriteLine($"Printer Status is -- > {str}");

                        // Write Error check code Here!

                        break;
                    }
                }




                while (true)
                {
                    Thread.Sleep(100);
                    str = SgdOverTcp_GetRfidLogEntries(ipAddress);
                    //Debug.WriteLine($"Length is -- > {str.IndexOf(t)}");
                    if (str.IndexOf(t) > -1)
                    {
                        Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Recieved rfid logs.");
                        Debug.WriteLine($"RFID log is -- > {str}");
                        break;
                    }
                }

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Delete set rfid log.");
                SgdOverTcp_DelRfidLogEntries(ipAddress);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :End process.");



            }

        }


        // Get Printer Status - printer.GetCurrentStatus();

        private void button4_Click(object sender, EventArgs e)
        {

            Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Start getting printer status.");

            Connection connection = new TcpConnection(ipAddress, TcpConnection.DEFAULT_ZPL_TCP_PORT);

            try
            {
                connection.Open();
                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                PrinterStatus printerStatus = printer.GetCurrentStatus();

                if (printerStatus.isReadyToPrint) { Debug.WriteLine("Ready To Print"); }
                else if (printerStatus.isRibbonOut) { Debug.WriteLine("Cannot Print. Ribbon Out."); }
                else if (printerStatus.isHeadOpen) { Debug.WriteLine("Cannot Print. Print Head Open."); }
                else if (printerStatus.isPaperOut) { Debug.WriteLine("Cannot Print. Paper Out."); }
                else if (printerStatus.isPaused) { Debug.WriteLine("Cannot Print. Printer Paused. "); }
                else { Debug.WriteLine("Cannot Print."); }
                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Recieved printer status.");
            }
            finally
            {
                connection.Close();
            }
        }



        // Get Printer Status - device.host_status
        private void button5_Click(object sender, EventArgs e)
        {
            string str;

            // Get Printer Status 
            Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Start getting printer status.");
            while (true)
            {
                Thread.Sleep(100);
                str = SgdOverTcp_GetPrinterStatus(ipAddress);
                //Debug.WriteLine($"Length is -- > {str.IndexOf(t)}");
                if (str != null)
                {
                    Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Recieved printer status.");
                    Debug.WriteLine($"Printer Status is -- > {str}");

                    // Write Error check code Here!

                    break;
                }
            }
        }

        // Syslog - Erase Syslog Entires
        private void button6_Click(object sender, EventArgs e)
        {
            string str;
            str = SgdOverTcp_EraseSyslogEntries(ipAddress);
        }

        // Syslog - Enable Syslog
        private void button7_Click(object sender, EventArgs e)
        {
            string str;
            str = SgdOverTcp_EnableSyslog(ipAddress);

        }

        // Syslog - Get Syslog Entries
        private void button8_Click(object sender, EventArgs e)
        {
            string str;
            str = SgdOverTcp_GetSyslogEntries(ipAddress);

        }

        // Read RFID tags by ZPL
        private void SendZplOverTcp(string theIpAddress,string zpl)
        {
            // Instantiate connection for ZPL TCP port at given address
            Connection thePrinterConn = new TcpConnection(theIpAddress, TcpConnection.DEFAULT_ZPL_TCP_PORT);

            try
            {
                // Open the connection - physical connection is established here.
                thePrinterConn.Open();

                // This example prints "This is a ZPL test." near the top of the label.
                string zplData = zpl;


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
        public static string SgdOverTcp_GetRfidLogEntries(string ipAddress)
        {
            int port = 9100;
            Connection printerConnection = new TcpConnection(ipAddress, port);
            string sgdResult = "";
            try
            {
                printerConnection.Open();
                sgdResult = SGD.GET("rfid.log.entries", printerConnection);
                //Debug.WriteLine($"RFID log is -- > {sgdResult}");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }

            return sgdResult;

        }


        // Get Printer Status
        public static string SgdOverTcp_GetPrinterStatus(string ipAddress)
        {
            int port = 9100;
            Connection printerConnection = new TcpConnection(ipAddress, port);
            string sgdResult = "";
            try
            {
                printerConnection.Open();
                sgdResult = SGD.GET("device.host_status", printerConnection);
                //Debug.WriteLine($"Printer status is -- > {sgdResult}");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }

            return sgdResult;

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


        public static string SgdOverTcp_EraseSyslogEntries(string ipAddress)
        {
            int port = 9100;
            string sgdResult = "";
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                SGD.DO("device.syslog.clear_log", "", printerConnection);
                sgdResult = SGD.GET("device.syslog.entries", printerConnection);
                Debug.WriteLine($"SGD device.syslog.entries is {sgdResult}.[End]");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
            return sgdResult;

        }


        public static string SgdOverTcp_EnableSyslog(string ipAddress)
        {
            int port = 9100;
            string sgdResult = "";
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                SGD.SET("device.syslog.enable", "on", printerConnection);
                SGD.SET("device.syslog.save_local_file", "yes", printerConnection);
                SGD.SET("device.syslog.log_max_file_size", "10000", printerConnection);
                SGD.SET("device.syslog.configuration", "ERROR,LOCAL", printerConnection);
                /* Selections of severity levels
                • emerg
                • alert
                • crit
                • err
                • warning
                • notice
                • info
                • debug
                 */
                sgdResult = SGD.GET("device.syslog.entries", printerConnection);
                Debug.WriteLine($"SGD device.syslog.entries is {sgdResult}.[End]");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
            return sgdResult;

        }


        public static string SgdOverTcp_GetSyslogEntries(string ipAddress)
        {
            int port = 9100;
            string sgdResult = "";
            Connection printerConnection = new TcpConnection(ipAddress, port);
            try
            {
                printerConnection.Open();
                sgdResult = SGD.GET("device.syslog.entries", printerConnection);
                Debug.WriteLine($"SGD device.syslog.entries is {sgdResult}.[End]");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                printerConnection.Close();
            }
            return sgdResult;

        }




    }
}