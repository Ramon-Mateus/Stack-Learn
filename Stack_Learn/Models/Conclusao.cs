using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Models
{
    public class Conclusao
    {
        public long ConclusaoId { get; set; }
        public Boolean Concluido { get; set; }

        public long? AlunoId { get; set; }
        public long? AulaId { get; set; }

        public Aluno Aluno { get; set; }
        public Aula Aula { get; set; }
    }
}