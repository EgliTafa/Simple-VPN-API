using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simple_VPN_API.Data.Interfaces;
using Simple_VPN_API.Models;
using System.Security.Claims;

namespace Simple_VPN_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class VpnController : ControllerBase
    {
        private readonly IVpnService _vpnService;
        private readonly UserManager<User> _userManager;

        public VpnController(IVpnService vpnRepository, UserManager<User> userManager)
        {
            _vpnService = vpnRepository;
            _userManager = userManager;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> Connect([FromBody] bool useTcp)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);


            var result = await _vpnService.ConnectAsync(useTcp);
            if (result)
            {
                
                return Ok("VPN connected successfully.");
            }

          
            return BadRequest("Failed to connect VPN.");
        }

        [HttpPost("disconnect")]
        public async Task<IActionResult> Disconnect()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

        


            var result = await _vpnService.DisconnectAsync();
            if (result)
            {
                return Ok("VPN disconnected successfully.");
            }

            return BadRequest("Failed to disconnect VPN.");
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var status = await _vpnService.GetStatusAsync();
            return Ok(status);
        }
    }
}
