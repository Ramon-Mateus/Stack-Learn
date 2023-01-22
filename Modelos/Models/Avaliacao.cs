using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Modelos.Models
{
    public class Avaliacao
    {
        public long AvaliacaoId { get; set; }

        public DateTime? Data_hora { get; set; }

        [Range(0, 5)]
        [Required(ErrorMessage = "Coloque no mínimo 0 e no máximo 5 estrelas.")]
        [DisplayName("NOTA : entre 0 e 5 estrelas")]
        public long Nota { get; set; }

        [DisplayName("COMENTÁRIO")]
        public string Comentario { get; set; }
        public string AlunoNome { get; set; }

        public long? CursoId { get; set; }
        public long? AlunoId { get; set; }

        public Aluno Aluno { get; set; }
        public Curso Curso { get; set; }

        public CursosUsuarios curso_usuario { get; set; }

    }
}