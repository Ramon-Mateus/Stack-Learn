using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Models
{
    public class Conclusao
    {
        public long? ConclusaoId { get; set; }
        public Boolean Concluido { get; set; }


        public virtual ICollection<Aula> Aulas { get; set; }

        public long? AlunoId { get; set; }
        public Aluno Aluno { get; set; }
    }
}
