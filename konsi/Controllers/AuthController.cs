using konsi.Models;
using konsi.Services;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Provider.RabbitMQ.Interfaces;

namespace konsi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    { 
        private readonly ILogger<AuthController> _logger; 
        private readonly AuthService _authService;
         
        public AuthController(ILogger<AuthController> logger, AuthService authService)
        {
            _logger = logger; 
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Auth()
        {
            var user = new UserRequest();
            await _authService.Authentication(user);
            return Ok(); 
        }
    }
}
