using Simple_VPN_API.Data.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Simple_VPN_API.Repositories
{
    public class VpnService : IVpnService
    {
        private Process openVpnProcess;
        private readonly string OpenVpnPath = "C:\\Program Files\\OpenVPN\\bin\\openvpn.exe";
        private readonly string SimpleVpnUdpPath = Path.Combine(AppContext.BaseDirectory, "OpenVpnConfigs", "SimpleVpn.ovpn");
        private readonly string SimpleVpnTcpPath = Path.Combine(AppContext.BaseDirectory, "OpenVpnConfigs", "SimpleVpn_TCP.ovpn");
        private readonly ILogger<VpnService> _logger;

        private readonly string vpnPublicIp = "161.35.14.130"; 

        public VpnService(ILogger<VpnService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> ConnectAsync(bool useTcp)
        {
            string configFilePath = useTcp ? SimpleVpnTcpPath : SimpleVpnUdpPath;

            if (string.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath))
            {
                _logger.LogError($"Config file path is invalid or missing: {configFilePath}");
                return false;
            }

            if (openVpnProcess != null && !openVpnProcess.HasExited)
            {
                _logger.LogWarning("An existing OpenVPN process is already running. Attempting to stop it before reconnecting.");
                await DisconnectAsync();
            }

            try
            {
                openVpnProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = OpenVpnPath,
                        Arguments = $"--config \"{configFilePath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    },
                    EnableRaisingEvents = true
                };

                openVpnProcess.OutputDataReceived += OpenVpnProcess_OutputDataReceived;
                openVpnProcess.ErrorDataReceived += OpenVpnProcess_ErrorDataReceived;
                openVpnProcess.Exited += OpenVpnProcess_Exited;

                _logger.LogInformation($"Duke u përpjekur të nis procesin OpenVPN me konfigurimin: {configFilePath}");

                // Nis procesin OpenVPN
                bool started = openVpnProcess.Start();
                if (!started)
                {
                    _logger.LogError("Dështoi të nisë procesi OpenVPN. Procesi nuk filloi.");
                    return false;
                }

                openVpnProcess.BeginOutputReadLine();
                openVpnProcess.BeginErrorReadLine();

                _logger.LogInformation("Procesi OpenVPN u nis me sukses.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Gabim gjatë nisjes së procesit OpenVPN: {ex.Message}");
                return false;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error starting OpenVPN process: {ex.Message}");
                if (openVpnProcess != null)
                {
                    string errorOutput = await openVpnProcess.StandardError.ReadToEndAsync();
                    _logger.LogError($"OpenVPN process error output: {errorOutput}");

                    openVpnProcess.Dispose();
                    openVpnProcess = null;
                }
                return false;
            }
        }


        public async Task<bool> DisconnectAsync()
        {
            try
            {
                if (openVpnProcess != null && !openVpnProcess.HasExited)
                {
                    _logger.LogInformation("Attempting to disconnect the VPN...");

                    openVpnProcess.Kill();
                    openVpnProcess.WaitForExit();
                    _logger.LogInformation("VPN process has exited.");

                    // Ensure disposal of the process
                    openVpnProcess.Dispose();
                    openVpnProcess = null;
                }
                else
                {
                    _logger.LogWarning("Disconnect called, but the VPN process was not running.");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error disconnecting VPN process: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetStatusAsync()
        {
            if (openVpnProcess != null && !openVpnProcess.HasExited)
            {
                string currentIp = await GetExternalIpAddressAsync();

                if (IsVpnIp(currentIp))
                {
                    return "Connected";
                }
                else
                {
                    _logger.LogWarning("OpenVPN process is running, but IP does not match the VPN IP.");
                    return "Disconnected";
                }
            }
            return "Disconnected";
        }

        // Method to get the current external IP address
        private async Task<string> GetExternalIpAddressAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Use a public API to get the external IP address
                    string ip = await client.GetStringAsync("https://api.ipify.org");
                    return ip;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve external IP address: {ex.Message}");
                return string.Empty;
            }
        }

        // Method to check if the IP address is the VPN's public IP
        private bool IsVpnIp(string ipAddress)
        {
            return ipAddress == vpnPublicIp;
        }

        private void OpenVpnProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                _logger.LogInformation($"Output: {e.Data}");
            }
        }

        private void OpenVpnProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                _logger.LogError($"Error: {e.Data}");
            }
        }

        private void OpenVpnProcess_Exited(object sender, EventArgs e)
        {
            _logger.LogWarning("OpenVPN process exited.");
            openVpnProcess = null;
        }
    }
}
