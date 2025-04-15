using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Viagem.Application.Dtos
{
    public class RotaDto
    {
        public int Id { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public decimal Valor { get; set; }

    }
}