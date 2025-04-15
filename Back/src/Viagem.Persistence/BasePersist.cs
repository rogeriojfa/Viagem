using System.Threading.Tasks;
using Viagem.Persistence.Contextos;
using Viagem.Persistence.Contratos;

namespace Viagem.Persistence
{
    public class BasePersist : IBasePersist
    {
        private readonly ViagemContext _context;
        public BasePersist(ViagemContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.AddAsync(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}