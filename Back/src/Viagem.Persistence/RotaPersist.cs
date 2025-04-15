using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viagem.Domain;
using Viagem.Persistence.Contextos;
using Viagem.Persistence.Contratos;
using Viagem.Persistence.Models;

namespace Viagem.Persistence
{
    public class RotaPersist : IRotaPersist
    {
        private readonly ViagemContext _context;
        public RotaPersist(ViagemContext context)
        {
            _context = context;
            // _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<IQueryable<Rota>> GetAllRotasAsync()
        {
            IQueryable<Rota> query =  _context.Rotas.AsNoTracking().OrderBy(e => e.Id);

            return query;
        }

        public async Task<Rota> GetRotaByIdAsync(int rotaId)
        {
            IQueryable<Rota> query = _context.Rotas;

           
            query = query.AsNoTracking().OrderBy(e => e.Id)
                         .Where(e => e.Id == rotaId);

            return await query.FirstOrDefaultAsync();
        }
    }
}