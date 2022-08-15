using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Models
{
    public class Professor
    {
        public long ProfessorId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }
    }
}