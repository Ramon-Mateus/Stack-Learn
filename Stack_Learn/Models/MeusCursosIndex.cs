using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Models
{
    public class MeusCursosIndex
    {
        public List<Curso> em_andamento { get; set; }
        public List<Curso> concluido { get; set; }

        public long? MeusCursosIndexId { get; set; }
    }
}