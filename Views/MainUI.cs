using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System;

namespace NTPTimeUP
{
    public partial class MainUI : Form
    {
        Process? process;
        private DateTime cachedNetworkTime;
private bool hasCachedNetworkTime = false;

        public MainUI() => InitializeComponent();

        static DateTime GetNetworkTime()
        {
            //default Windows time server
            const string ntpServer = "a.st1.ntp.br";

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;

            //The UDP port number assigned to NTP is 123
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            //NTP uses UDP

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);

                //Stops code hang if NTP is blocked
                socket.ReceiveTimeout = 3000;

                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**UTC** time
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        bool SetTimeCMD(string clock)
        {
            // Validate the input clock value
            if (!DateTime.TryParse(clock, out DateTime dateTime))
            {
                Console.WriteLine("Invalid clock value");
                return false;
            }

            // If the process has not been created yet, create it
            if (process == null)
            {
                process = new Process();
                process.StartInfo.Verb = "runas";
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }

            // Use time command to set the clock
            process.StandardInput.WriteLine("time");
            process.StandardInput.Flush();

            process.StandardInput.WriteLine(clock);
            process.StandardInput.Flush();
            process.StandardInput.Close();

            // Free all the resources on process
            process.Close();

            return true;
        }

        private void BtnSyncNTP_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTime;
                if (!hasCachedNetworkTime)
                {
                    dateTime = GetNetworkTime();
                    cachedNetworkTime = dateTime;
                    hasCachedNetworkTime = true;
                }
                else
                {
                    dateTime = cachedNetworkTime;
                }

                var clock = dateTime.ToString("HH:mm:ss");

                if (SetTimeCMD(clock))
                {
                    MessageBox.Show("Horário sincronizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Houve um erro ao buscar o horário correto, verifique sua conexão e tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}