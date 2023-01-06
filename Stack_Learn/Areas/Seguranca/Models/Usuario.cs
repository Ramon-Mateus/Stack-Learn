using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stack_Learn.Areas.Seguranca.Models
{
    public class Usuario : IdentityUser
    {
        public long? AlunoId { get; set; }
        public long? ProfessorId { get; set; }
    }
}