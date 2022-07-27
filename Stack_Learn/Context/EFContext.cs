using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
//using System.Data.Entity;
using Stack_Learn.Context;
using Stack_Learn.Models;

namespace Stack_Learn.Context
{
    public class EFContext : DbContext
    {
        public EFContext() : base("Asp_Net_MVC_CS") { }
        public DbSet<Curso> Cursos { get; set; }
    }
}