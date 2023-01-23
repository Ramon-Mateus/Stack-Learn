using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Modelos.Models
{
    public class Aluno
    {
        public long AlunoId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "No mínimo 6 caracteres")]
        public string Login { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "No mínimo 6 caracteres")]
        public string Senha { get; set; }
        public bool Assinatura { get; set; }
        public CursosUsuarios curso_usuario { get; set; }
        public string Id_do_usuario { get; set; }

        public ICollection<Avaliacao> Avaliacoes { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Conclusao> Conclusoes { get; set; }

    }
}