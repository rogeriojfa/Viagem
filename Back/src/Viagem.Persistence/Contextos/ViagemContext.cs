using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Viagem.Domain;

namespace Viagem.Persistence.Contextos
{
    public class ViagemContext : IdentityDbContext
                                                       
                                                      
    {
        public ViagemContext(DbContextOptions<ViagemContext> options)
            : base(options) { }
        public DbSet<Rota> Rotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}