using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelos.Models
{
    public class Pedido
    {
        public long PedidoId { get; set; }
        public Boolean Pago { get; set; }
        public Boolean Cancelamento { get; set; }
        public DateTime? Data_cancelamento { get; set; }
        public DateTime? Data_Pagamento { get; set; }
        public double Valor_Total { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public long? AlunoId { get; set; }
        public Aluno Aluno { get; set; }
    }
}