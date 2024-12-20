﻿namespace Simple_VPN_API.Data.Interfaces
{
    public interface IVpnService
    {
        Task<bool> ConnectAsync(bool useTcp);
        Task<bool> DisconnectAsync();
        Task<string> GetStatusAsync();
    }
}
