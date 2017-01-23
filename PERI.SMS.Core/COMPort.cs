using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Threading;

namespace PERI.SMS.Core
{
    /// <summary>
    /// Defines the COMPort to where the app should connect
    /// <see cref="http://www.codeproject.com/Articles/38705/Send-and-Read-SMS-through-a-GSM-Modem-using-AT-Com"/>
    /// </summary>
    public class COMPort
    {
        private static AutoResetEvent readNow = new AutoResetEvent(false);
        private static AutoResetEvent receiveNow;

        /// <summary>
        /// Opens a COMPort
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="readTimeout"></param>
        /// <param name="writeTimeout"></param>
        /// <returns>SerialPort</returns>
        public static SerialPort OpenPort(string portName, int baudRate, int dataBits, int readTimeout, int writeTimeout)
        {
            receiveNow = new AutoResetEvent(false);
            SerialPort port = new SerialPort();
            port.PortName = portName;
            port.BaudRate = baudRate;
            port.DataBits = dataBits;
            port.StopBits = StopBits.One;
            port.Parity = Parity.None;
            port.ReadTimeout = readTimeout;
            port.WriteTimeout = writeTimeout;
            port.Encoding = Encoding.GetEncoding("iso-8859-1");
            port.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            port.Open();
            port.DtrEnable = true;
            port.RtsEnable = true;

            return port;
        }

        /// <summary>
        /// DataReceive event of the COMPort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
                receiveNow.Set();
        }

        /// <summary>
        /// Gets the response of the COMPort
        /// </summary>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <returns>string</returns>
        private static string ReadResponse(SerialPort port, int timeout)
        {
            string buffer = string.Empty;
            try
            {
                do
                {
                    if (receiveNow.WaitOne(timeout, false))
                    {
                        string t = port.ReadExisting();
                        buffer += t;
                    }
                    else
                    {
                        if (buffer.Length > 0)
                            throw new ApplicationException("Response received is incomplete.");
                        else
                            throw new ApplicationException("No data received from phone.");
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }
                
        /// <summary>
        /// Exceutes an ATCommand
        /// </summary>
        /// <param name="port"></param>
        /// <param name="command"></param>
        /// <param name="responseTimeout"></param>
        /// <param name="errorMessage"></param>
        /// <returns>string</returns>
        public static string ExecuteATCommand(SerialPort port, string command, int responseTimeout, string errorMessage)
        {
            port.DiscardOutBuffer();
            port.DiscardInBuffer();
            receiveNow.Reset();
            port.Write(command + "\r");

            string input = ReadResponse(port, responseTimeout);
            if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                throw new ApplicationException("No success message was received.");

            return input;
        }   
    }
}
