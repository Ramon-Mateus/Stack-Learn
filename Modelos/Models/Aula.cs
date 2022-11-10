using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelos.Models
{
    public class Aula
    {
        public long? AulaId { get; set; }
        public int Ordem { get; set; }
        public string Titulo { get; set; }
        public long Duracao { get; set; }

        public long? CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}