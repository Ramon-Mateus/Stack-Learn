using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelos.Models;

namespace Stack_Learn.Models
{
    public class CursosUsuarios
    {
        public long? AlunoId { get; set; }
        public List<Curso> Cursos {get; set;}
    }
}