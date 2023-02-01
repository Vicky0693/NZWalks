using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class AuthController : Controller    
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository ,ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>LoginAsync(Model.DTO.LoginRequest loginRequest)
        {

            // Check if User is Authenticate

            // Check usernameand Password

            var user = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (user != null)
            {
                //generate request
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);

            }
            return BadRequest("Username OR Password is invalide");





        }
    }
}
