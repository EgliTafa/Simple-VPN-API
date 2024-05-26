using Simple_VPN_API.Data.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Simple_VPN_API.Repositories
{
    public class VpnRepository : IVpnRepository
    {
        private Process openVpnProcess;
        private readonly string OpenVpnPath = "C:\\Program Files\\OpenVPN\\bin\\openvpn.exe";
        private readonly string SimpleVpnUdpPath = Path.Combine(AppContext.BaseDirectory, "OpenVpnConfigs", "SimpleVpn.ovpn");
        private readonly string SimpleVpnTcpPath = Path.Combine(AppContext.BaseDirectory, "OpenVpnConfigs", "SimpleVpn_TCP.ovpn");

        public async Task<bool> ConnectAsync(bool useTcp)
        {
            string configFilePath = useTcp ? SimpleVpnTcpPath : SimpleVpnUdpPath;

            if (string.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath))
            {
                Console.WriteLine($"Config file path is invalid: {configFilePath}");
                return false;
            }

            try
            {
                openVpnProcess = new Process();
                openVpnProcess.StartInfo.FileName = OpenVpnPath;
                openVpnProcess.StartInfo.Arguments = $"--config \"{configFilePath}\"";
                openVpnProcess.StartInfo.UseShellExecute = false;
                openVpnProcess.StartInfo.RedirectStandardOutput = true;
                openVpnProcess.StartInfo.RedirectStandardError = true;
                openVpnProcess.OutputDataReceived += OpenVpnProcess_OutputDataReceived;
                openVpnProcess.ErrorDataReceived += OpenVpnProcess_ErrorDataReceived;
                openVpnProcess.Start();
                openVpnProcess.BeginOutputReadLine();
                openVpnProcess.BeginErrorReadLine();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting OpenVPN process: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DisconnectAsync()
        {
            try
            {
                if (openVpnProcess != null && !openVpnProcess.HasExited)
                {
                    openVpnProcess.Kill();
                    openVpnProcess = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disconnecting OpenVPN process: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetStatusAsync()
        {
            if (openVpnProcess != null && !openVpnProcess.HasExited)
            {
                return "Connected";
            }
            return "Disconnected";
        }

        private void OpenVpnProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine($"Output: {e.Data}");
            }
        }

        private void OpenVpnProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine($"Error: {e.Data}");
            }
        }
    }
}
