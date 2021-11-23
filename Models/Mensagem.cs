using System.Collections.Generic;
using System.Linq;
using MimeKit;

namespace UsuariosApi.Models
{
    public class Mensagem
    {
        public List<MailboxAddress> Destinatario { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }

        public Mensagem(IEnumerable<string> destinatario, string assunto, int usuarioId, string codigo)
        {
            //primeiro instanciamos uma lista de Mailbox em nossa propriedade já existente
            Destinatario = new List<MailboxAddress>();
            //depois adicionamos a ela um novo elemento, que é um destinatario que está na nossa lista de destinarario recebida por parametro
            Destinatario.AddRange(destinatario.Select(dest => new MailboxAddress(dest)));
            Assunto = assunto;
            Conteudo = $"http://localhost:6000/ativa?UsuarioId={usuarioId}&CodigoDeAtivacao={codigo}" ;
        }
    }
}