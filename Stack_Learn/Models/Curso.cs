using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Models
{
    public class Curso
    {
        public long CursoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Qtd_Aulas { get; set; }
        public double Preco { get; set; }
        
        public long? ProfessorId { get; set; }
        public long? CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
        public Professor Professor { get; set; }

        public virtual ICollection<Aula> Aulas { get; set; }
    }
}