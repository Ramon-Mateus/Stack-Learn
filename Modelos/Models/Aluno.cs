using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelos.Models
{
    public class Aluno
    {
        public long AlunoId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Assinatura { get; set; }

        public ICollection<Avaliacao> Avaliacoes { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Conclusao> Conclusoes { get; set; }

    }
}