using System;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
  public class EmailService
  {
    private IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public void EnviarEmail(string[] destinatario, 
                            string assunto, 
                            int usuarioId, 
                            string code)
    {
        Mensagem mensagem = new Mensagem(destinatario, assunto, usuarioId, code);
        var mensagemDeEmail = CriaCorpoDoEmail(mensagem);
        Enviar(mensagemDeEmail);
    }

    private void Enviar(MimeMessage mensagemDeEmail)
    {
      using(var client = new SmtpClient())
      {
        try
        {
          //primeiro parametro é o SmtpServer, o segundo a porta e o terceiro a opção de utilizar SSL ou não
          client.Connect(_configuration.GetValue<string>("EmailSettings:SmtpServer"),
              _configuration.GetValue<int>("EmailSettings:Port"), true);
          client.AuthenticationMechanisms.Remove("XOUATH2");
          //primeiro parametro é o remetente, segundo parametro é a senha.
          client.Authenticate(_configuration.GetValue<string>("EmailSettings:From"), _configuration.GetValue<string>("EmailSettings:Password"));
          client.Send(mensagemDeEmail);
        }
        catch
        {
          throw;
        }
        finally
        {
          client.Disconnect(true);
          client.Dispose();
        }
      }
    }

    private MimeMessage CriaCorpoDoEmail(Mensagem mensagem)
    {
      var mensagemDeEmail = new MimeMessage();
      //adicionando o FROM, ou seja, REMETENTE
      mensagemDeEmail.From.Add(new MailboxAddress(
            _configuration.GetValue<string>("EmailSettings:From")));
      mensagemDeEmail.To.AddRange(mensagem.Destinatario);
      mensagemDeEmail.Subject = mensagem.Assunto;
      mensagemDeEmail.Body = new TextPart(MimeKit.Text.TextFormat.Text)
      {
        Text = mensagem.Conteudo
      };

      return mensagemDeEmail;
    }
  }
}