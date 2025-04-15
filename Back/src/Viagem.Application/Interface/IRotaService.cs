using Viagem.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Viagem.Persistence.Models;
using System.Collections.Generic;

namespace Viagem.Application.Contratos
{
    public interface IRotaService
    {
        Task<RotaDto> AddRotas(RotaDto model);
        Task<RotaDto> UpdateRota(int rotaId, RotaDto model);
        Task<bool> DeleteRota(int rotaId);
        Task<RotaDto[]> GetAllRotasAsync();
        Task<RotaDto> GetRotaByIdAsync(int rotaId);
        Task<(List<string>, decimal)> BuscarMelhorRotaAsync(string origem, string destino);
    }
}