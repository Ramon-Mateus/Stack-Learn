using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Models
{
    public class Avaliacao
    {
        public long AvaliacaoId { get; set; }

        public DateTime? Data_hora { get; set; }
        public long Nota { get; set; }
        public string Comentario { get; set; }

        public long? CursoId { get; set; }
        public long? AlunoId { get; set; }

        public Aluno Aluno { get; set; }
        public Curso Curso { get; set; }
    }
}