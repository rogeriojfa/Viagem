using System.Linq;
using System.Threading.Tasks;
using Viagem.Domain;

namespace Viagem.Persistence.Contratos
{
    public interface IRotaPersist
    {
        Task<IQueryable<Rota>> GetAllRotasAsync();
        Task<Rota> GetRotaByIdAsync(int rotaId);
    }
}