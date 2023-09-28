using Microsoft.AspNetCore.Mvc;

using collabBackend.Services;
using collabBackend.Models;

namespace collabBackend.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        ILoginService LoginService;

        public LoginController(ILoginService loginService)
        {
            LoginService = loginService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(LoginService.Get());
        }

        [HttpPost]
      
        public IActionResult Post([FromBody]LoginRequestModel loginDto)
        {
            try
            {
                var result = LoginService.ValidateCredentials(loginDto.Email, loginDto.Password);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                return StatusCode(500, new
                {
                    success = false,
                    token = "",
                    error = "Ocurrió un error en el servidor.",
                    usuario = ""
                });

            }
        }

    }
}
