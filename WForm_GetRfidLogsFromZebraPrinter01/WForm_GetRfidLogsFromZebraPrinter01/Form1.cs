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
        string ipAddress = "192.168.4.55";
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



                zplStrings = "^XA^RFW,H^FD0000000000A" + i.ToString() + "^XZ";
                // zplStrings = @"^XA^MMC^CWK,E:IPAG.TTF^FS^CI28^PW1146^LL319^LS0^FT21,136^AK,41,41^FD現^FS^FT21,184^AK,41,41^FD品^FS^FT21,231^AK,41,41^FD票^FS^FT165,68^AK,50,50^FDBXT155-38-10F-5C=1^FS^FT0,117^FB389,1,12,R^AK,45,45^FD20^FS^FT0,117^FB708,1,12,R^AK,45,45^FD20^FS^FT0,113^FB933,1,11,R^AK,41,41^FD1/1^FS^FT165,150^AK,29,29^FD次工程協力会社 スリーケ株^FS^FT248,209^AK,50,50^FD2022/05/24^FS^FT148,255^AK,41,41^FDマニホールドベース　^FS^FT164,195^FB982,1,7,C^AK,29,29^FDＡＳ^FS^FT148,291^AK,33,33^FD02001 飯塚鉄工（株）^FS^FT791,291^AK,33,33^FD519999635^FS^FT774,190^AK,25,25^FD^FS^FT644,242^AK,29,29^FD023AAC62-66^FS^FT0,60^FB1121,1,11,R^AK,41,41^FD2^FS^FT0,92^FB1121,1,8,R^AK,33,33^FD8461^FS^FT992,234^BQN,2,4^FH\^FDMA,BXT155-38-10F-5C=1$0000020$003020012209051519999635010001$^FS^FO602,159^GB106,47,5^FS^FT709,291^AK,33,33^FD注番^FS^FT709,194^AK,29,29^FD手配^FS^FT0,60^FB1068,1,11,R^AK,41,41^FD設変^FS^FT573,246^AK,33,33^FD工号^FS^FT71,246^AK,33,33^FD品名^FS^FT71,291^AK,33,33^FD社名^FS^FT71,198^AK,33,33^FD納入日^FS^FT71,154^AK,33,33^FD場所^FS^FT71,111^AK,37,37^FD数量^FS^FT71,56^AK,37,37^FD品番^FS^FT957,295^AK,58,58^FDH^FS^FT988,282^FB158,1,7,C^AK,29,29^FDＳＭＣ^FS^FO951,245^GB41,51,5^FS^FT969,84^AK,25,25^FD請求元^FS^FT933,84^AK,25,25^FD袋^FS^FT933,114^AK,25,25^FD箱^FS^FT390,112^AK,33,33^FD個^FS^FT709,112^AK,33,33^FD個^FS^FT449,112^AK,33,33^FD計^FS^LRY^FO21,53^GB0,224,41^FS^LRN^LRY^FO1013,249^GB94,0,44^FS^LRN^PQ1,,,Y^RFW,H^FD534d43001efe909301000001^FS^XZ";
                //Debug.WriteLine(zplStrings);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Label #" + i);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Set rfid log.");
                SgdOverTcp_SetRfidLogEntries(ipAddress);

                Debug.WriteLine(String.Format("{0:yyyy年MM月dd日(ddd) HH時mm分ss秒fff}", DateTime.Now) + " :Send ZPL.");
                SendZplOverTcp(ipAddress, zplStrings);





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