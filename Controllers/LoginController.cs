using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Data.Requests;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult LogaUsuario(LoginRequest request)
        {
            Result resultado = _loginService.LogaUsuario(request);
            if(resultado.IsFailed){
                //nesse caso, exibiremos "Login falhou", como está no LoginService
                return Unauthorized(resultado.Errors);
            }
            //no caso de sucesso, o Successes estará preenchido com o token
            return Ok(resultado.Successes);
        }
    }
}