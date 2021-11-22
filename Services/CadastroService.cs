using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.Dtos;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class CadastroService
    {
        private IMapper _mapper;
        private UserManager<IdentityUser<int>> _userManager;

        public CadastroService(IMapper mapper, UserManager<IdentityUser<int>> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public Result CadastraUsuario(CreateUsuarioDto createDto)
        {
            //primeiro mapeamos de Dto para Usuario
            Usuario usuario = _mapper.Map<Usuario>(createDto);
            //depois mapeamos para um IdentityUser, que é o padrão salvo no banco de dados
            IdentityUser<int> usuarioIdentity = _mapper.Map<IdentityUser<int>>(usuario);
            //aqui, estamos fazendo a operação de criação do usuário, o primeiro parametro é o usuario, o segundo é a senha. 
            Task<IdentityResult> resultadoIdentity = _userManager.CreateAsync(usuarioIdentity, createDto.Password);
            if(resultadoIdentity.Result.Succeeded){
                return Result.Ok();
            }
            return Result.Fail("Falha ao cadastrar usuário");
        }
    }
}