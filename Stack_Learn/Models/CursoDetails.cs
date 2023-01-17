using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Models
{
    public class CursoDetails
    {
        public Curso curso { get; set; }
        public IQueryable<Curso> cursos { get; set; }
        public CursosUsuarios cursos_usuarios { get; set; }

    }
}