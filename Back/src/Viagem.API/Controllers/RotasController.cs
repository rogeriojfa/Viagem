using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viagem.Application.Contratos;
using Microsoft.AspNetCore.Http;
using Viagem.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Viagem.Persistence.Models;
using Viagem.Api.Helpers;
using System.Collections.Generic;

namespace Viagem.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _rotaService;
        private readonly IUtil _util;


        public RotasController(IRotaService rotaService,
                                 IUtil util)
        {
            _util = util;
            _rotaService = rotaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var rotas = await _rotaService.GetAllRotasAsync();
                if (rotas == null) return NoContent();


                return Ok(rotas);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rotas. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var rota = await _rotaService.GetRotaByIdAsync(id);
                if (rota == null) return NoContent();

                return Ok(rota);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rotas. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(RotaDto model)
        {
            try
            {
                var rota = await _rotaService.AddRotas(model);
                if (rota == null) return NoContent();

                return Ok(rota);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar rotas. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, RotaDto model)
        {
            try
            {
                var rota = await _rotaService.UpdateRota(id, model);
                if (rota == null) return NoContent();

                return Ok(rota);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar rotas. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var rota = await _rotaService.GetRotaByIdAsync(id);
                if (rota == null) return NoContent();

                if (await _rotaService.DeleteRota(id))
                {
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar Rota.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rotas. Erro: {ex.Message}");
            }
        }

		[HttpGet("BuscarMelhorRota")]
		public async Task<IActionResult> ConsultarMelhorRota([FromQuery] string origem, [FromQuery] string destino)
		{
			var (caminho, custo) = await _rotaService.BuscarMelhorRotaAsync(origem.ToUpper(), destino.ToUpper());

			if (custo == -1)
				return NotFound("Rota não encontrada.");

			return Ok($"{string.Join(" - ", caminho)} ao custo de ${custo}");
		}
	}
}
