using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PERI.SMS.Core
{
    /// <summary>
    /// Where the SMS is being manipulated
    /// <see cref="http://www.codeproject.com/Articles/38705/Send-and-Read-SMS-through-a-GSM-Modem-using-AT-Com"/>
    /// </summary>
    public class SMS
    {
        /// <summary>
        /// Sends the SMS to a specific recipent
        /// </summary>
        /// <param name="port"></param>
        /// <param name="mobile"></param>
        /// <param name="message"></param>
        /// <returns>bool - Yes/No(Success)</returns>
        public static bool Send(SerialPort port, string mobile, string message)
        {
            try
            {
                string recievedData = COMPort.ExecuteATCommand(port, "AT", 300, "No phone connected");
                recievedData = COMPort.ExecuteATCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                String command = "AT+CMGS=\"" + mobile + "\"";
                recievedData = COMPort.ExecuteATCommand(port, command, 300, "Failed to accept phoneNo");
                command = message + char.ConvertFromUtf32(26) + "\r";
                recievedData = COMPort.ExecuteATCommand(port, command, 3000, "Failed to send message");

                return (recievedData.EndsWith("\r\nOK\r\n"));
            }
            catch (ApplicationException ex)
            {
                // Garbled response
                // Not enough balance or SIM is deactivated
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// <see cref="http://www.developershome.com/sms/readSmsByAtCommands.asp"/>
        /// </summary>
        public class Read
        {
            static Dictionary<string, int> dictstatus = new Dictionary<string, int>()
            {
                { "REC UNREAD", 1 },
                { "REC READ", 2 },
            };

            /// <summary>
            /// Initializes the Receiving process
            /// </summary>
            /// <param name="port"></param>
            static void Initialize(SerialPort port)
            {
                // Check connection
                COMPort.ExecuteATCommand(port, "AT", 300, "No phone connected");
                // Use message format "Text mode"
                COMPort.ExecuteATCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                // Use character set "PCCP437"
                //ExecCommand(port,"AT+CSCS=\"PCCP437\"", 300, "Failed to set character set.");
                // Select SIM storage
                COMPort.ExecuteATCommand(port, "AT+CPMS=\"SM\"", 300, "Failed to select message storage.");
                // Read the messages
            }

            /// <summary>
            /// Parses received messages into IEnumberable object
            /// </summary>
            /// <param name="input"></param>
            /// <returns>IEnumerable of Model.SMS</returns>
            static IEnumerable<Model.SMS> ParseMessages(string input)
            {
                List<Model.SMS> msgs = new List<Model.SMS>();

                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
                Match m = r.Match(input);

                while (m.Success)
                {
                    Model.SMS msg = new Model.SMS();
                    msg.ID = Guid.NewGuid();
                    msg.Status = dictstatus[m.Groups[2].Value];
                    msg.Mobile = m.Groups[3].Value;
                    
                    DateTime today;
                    if (!DateTime.TryParseExact(m.Groups[5].Value, "yy/MM/dd,H:mm:ss+32",CultureInfo.InvariantCulture, DateTimeStyles.None, out today))
                        today = DateTime.Now;
                    msg.SMSDate = today;

                    msg.Message = m.Groups[6].Value;
                    msgs.Add(msg);

                    m = m.NextMatch();
                }

                return msgs;
            }

            /// <summary>
            /// Gets all messages from modem
            /// </summary>
            /// <param name="port"></param>
            /// <returns>IEnumerable of Model.SMS</returns>
            public static IEnumerable<Model.SMS> All(SerialPort port)
            {
                Initialize(port);                
                string input = COMPort.ExecuteATCommand(port, "AT+CMGL=\"ALL\"", 5000, "Failed to read the messages.");
                return ParseMessages(input);
            }

            /// <summary>
            /// Gets all unread messages from modem
            /// </summary>
            /// <param name="port"></param>
            /// <returns>IEnumerable of Model.SMS</returns>
            public static IEnumerable<Model.SMS> AllUnread(SerialPort port)
            {
                Initialize(port);
                string input = COMPort.ExecuteATCommand(port, "AT+CMGL=\"REC UNREAD\"", 5000, "Failed to read the messages.");
                return ParseMessages(input);
            }
        }

        /// <summary>
        /// <see cref="http://www.developershome.com/sms/cmgdCommand.asp"/>
        /// </summary>
        public class Delete
        {
            /// <summary>
            /// Deletes all messages in modem
            /// </summary>
            /// <param name="port"></param>
            /// <returns>bool</returns>
            public static bool All(SerialPort port)
            {
                try
                {
                    string recievedData = COMPort.ExecuteATCommand(port, "AT", 300, "No phone connected");
                    recievedData = COMPort.ExecuteATCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                    String command = "AT+CMGD=1,4";
                    recievedData = COMPort.ExecuteATCommand(port, command, 300, "Failed to delete message");

                    return (recievedData.EndsWith("\r\nOK\r\n"));
                }
                catch (ApplicationException ex)
                {
                    // Garbled response
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            /// <summary>
            /// Deletes all read messages in modem
            /// </summary>
            /// <param name="port"></param>
            /// <returns>bool</returns>
            public static bool AllRead(SerialPort port)
            {
                try
                {
                    string recievedData = COMPort.ExecuteATCommand(port, "AT", 300, "No phone connected");
                    recievedData = COMPort.ExecuteATCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                    String command = "AT+CMGD=1,3";
                    recievedData = COMPort.ExecuteATCommand(port, command, 300, "Failed to delete message");

                    return (recievedData.EndsWith("\r\nOK\r\n"));
                }
                catch (ApplicationException ex)
                {
                    // Garbled response
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
