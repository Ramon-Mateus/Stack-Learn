using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Modelos.Models
{
    public class Professor
    {
        public long ProfessorId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "No mínimo 6 caracteres")]
        public string Login { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "No mínimo 6 caracteres")]
        public string Senha { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }
        public CursosUsuarios curso_usuario { get; set; }
        public string Id_do_usuario { get; set; }
    }
}