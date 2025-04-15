using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Viagem.Application.Dtos;
using Viagem.Application.Contratos;
using Viagem.Domain;
using Viagem.Persistence.Contratos;
using System.Collections.Generic;

namespace Viagem.Application
{
    public class RotaService : IRotaService
    {
        private readonly IBasePersist _basePersist;
        private readonly IRotaPersist _rotaPersist;
        private readonly IMapper _mapper;
        public RotaService(IBasePersist basePersist,
                             IRotaPersist rotaPersist,
                             IMapper mapper)
        {
            _basePersist = basePersist;
            _rotaPersist = rotaPersist;
            _mapper = mapper;
        }
        public async Task<RotaDto> AddRotas(RotaDto model)
        {
            try
            {
                var rota = _mapper.Map<Rota>(model);

                _basePersist.Add<Rota>(rota);

                if (await _basePersist.SaveChangesAsync())
                {
                    var rotaRetorno = await _rotaPersist.GetRotaByIdAsync(rota.Id);

                    return _mapper.Map<RotaDto>(rotaRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RotaDto> UpdateRota(int rotaId, RotaDto model)
        {
            try
            {
                var rota = await _rotaPersist.GetRotaByIdAsync(rotaId);
                if (rota == null) return null;

                model.Id = rota.Id;

                _mapper.Map(model, rota);

                _basePersist.Update<Rota>(rota);

                if (await _basePersist.SaveChangesAsync())
                {
                    var rotaRetorno = await _rotaPersist.GetRotaByIdAsync(rota.Id);

                    return _mapper.Map<RotaDto>(rotaRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRota(int rotaId)
        {
            try
            {
                var rota = await _rotaPersist.GetRotaByIdAsync(rotaId);
                if (rota == null) throw new Exception("Rota para delete não encontrado.");

                _basePersist.Delete<Rota>(rota);
                return await _basePersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RotaDto[]> GetAllRotasAsync()
        {
            try
            {
                var rotas = await _rotaPersist.GetAllRotasAsync();
                if (rotas == null) return null;

                var resultado = _mapper.Map<RotaDto[]>(rotas);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RotaDto> GetRotaByIdAsync(int rotaId)
        {
            try
            {
                var rota = await _rotaPersist.GetRotaByIdAsync(rotaId);
                if (rota == null) return null;

                var resultado = _mapper.Map<RotaDto>(rota);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<string>, decimal)> BuscarMelhorRotaAsync(string origem, string destino)
        {
            var resultado = await _rotaPersist.GetAllRotasAsync(); 

            var distancias = new Dictionary<string, decimal>();
            var anteriores = new Dictionary<string, string>();
            decimal infinitoPratico = 999_999_999m;

            var nos = resultado.Select(r => r.Origem).Union(resultado.Select(r => r.Destino)).Distinct().ToList();

            foreach (var no in nos)
                distancias[no] = infinitoPratico;

            distancias[origem] = 0;

            var visitados = new HashSet<string>();

            while (visitados.Count < nos.Count)
            {
                var noAtual = distancias
                    .Where(d => !visitados.Contains(d.Key))
                    .OrderBy(d => d.Value)
                    .FirstOrDefault();

                if (noAtual.Key == null || noAtual.Value == infinitoPratico)
                    break; // Nenhum nó acessível restante

                visitados.Add(noAtual.Key);

                var vizinhos = resultado.Where(r => r.Origem == noAtual.Key);

                foreach (var vizinho in vizinhos)
                {
                    var novaDist = distancias[noAtual.Key] + vizinho.Valor;
                    if (novaDist < distancias[vizinho.Destino])
                    {
                        distancias[vizinho.Destino] = novaDist;
                        anteriores[vizinho.Destino] = noAtual.Key;
                    }
                }
            }

            var caminho = new List<string>();
            string atual = destino;

            if (!anteriores.ContainsKey(destino) && origem != destino)
                return (new List<string>(), -1); // Caminho impossível

            while (atual != null && anteriores.ContainsKey(atual))
            {
                caminho.Insert(0, atual);
                atual = anteriores[atual];
            }

            if (atual == origem)
                caminho.Insert(0, origem);
            else if (origem != destino)
                return (new List<string>(), -1); // Caminho incompleto

            return (caminho, distancias[destino]);
        }
    }
}