using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Viagem.Application;
using Viagem.Domain;
using Viagem.Persistence.Contratos;
using Xunit;

namespace Viagem.Tests
{
    public class RotaServiceTests
    {
        public static IQueryable<Rota> GetAllRotas()
        {
            var rotas = new List<Rota>
            {
                new() { Origem = "GRU", Destino = "BRC", Valor = 10 },
                new() { Origem = "BRC", Destino = "SCL", Valor = 5 },
                new() { Origem = "GRU", Destino = "CDG", Valor = 75 },
                new() { Origem = "GRU", Destino = "SCL", Valor = 20 },
                new() { Origem = "GRU", Destino = "ORL", Valor = 56 },
                new() { Origem = "ORL", Destino = "CDG", Valor = 5 },
                new() { Origem = "SCL", Destino = "ORL", Valor = 20 }
            }.AsQueryable();

            return rotas;
        }



        [Fact]
        public void DeveRetornarTodasAsRotas()
        {
            var rotas = GetAllRotas();
            Assert.Equal(7, rotas.Count());
        }

        
        [Fact]
        public async void DeveEncontrarRotaDireta()
        {
            var mockRotaPersist = new Mock<IRotaPersist>();
            mockRotaPersist.Setup(r => r.GetAllRotasAsync()).ReturnsAsync(GetAllRotas);

            var mockBasePersist = new Mock<IBasePersist>();
            var mockMapper = new Mock<AutoMapper.IMapper>();

            var service = new RotaService(mockBasePersist.Object, mockRotaPersist.Object, mockMapper.Object);
            var (caminho, custo) = await service.BuscarMelhorRotaAsync("BRC", "SCL");

            Assert.Equal(new List<string> { "BRC", "SCL" }, caminho);
            Assert.Equal(5, custo);
        }

        [Fact]
        public async void DeveRetornarCustoMenos1QuandoNaoExisteRota()
        {
            var mockRotaPersist = new Mock<IRotaPersist>();
            mockRotaPersist.Setup(r => r.GetAllRotasAsync()).ReturnsAsync(GetAllRotas);

            var mockBasePersist = new Mock<IBasePersist>();
            var mockMapper = new Mock<AutoMapper.IMapper>();

            var service = new RotaService(mockBasePersist.Object, mockRotaPersist.Object, mockMapper.Object); 
            
            var (caminho, custo) =  await service.BuscarMelhorRotaAsync("XYZ", "SCL");

            Assert.Empty(caminho);
            Assert.Equal(-1, custo);
        }

        [Fact]
        public async Task BuscarMelhorRotaAsync_DeveRetornarRotaMaisBarata()
        {
            // Arrange
            var mockRotaPersist = new Mock<IRotaPersist>();
            mockRotaPersist.Setup(r => r.GetAllRotasAsync()).ReturnsAsync(GetAllRotas);

            var mockBasePersist = new Mock<IBasePersist>();
            var mockMapper = new Mock<AutoMapper.IMapper>();

            var service = new RotaService(mockBasePersist.Object, mockRotaPersist.Object, mockMapper.Object);

            // Act
            var (caminho, custo) = await service.BuscarMelhorRotaAsync("BRC", "SCL");

            // Assert
            Assert.Equal(new List<string> { "BRC", "SCL" }, caminho);
            Assert.Equal(5, custo);
        }

    }
}
