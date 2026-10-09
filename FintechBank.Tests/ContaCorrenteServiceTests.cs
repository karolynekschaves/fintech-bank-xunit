using Xunit;

namespace FintechBank.Tests
{
    public class ContaCorrenteServiceTests
    {
        private readonly ContaCorrenteService _contaCorrenteService;

        public ContaCorrenteServiceTests()
        {
            _contaCorrenteService = new ContaCorrenteService();
        }

        [Theory]
        [InlineData(6000.00, false, 0.00)]
        [InlineData(500.00, true, 0.00)]
        [InlineData(2500.00, false, 12.50)]
        [InlineData(300.00, false, 25.00)]
        public void CalcularTaxaManutencao_DeveRetornarTaxaCorreta(decimal saldo, bool ehEspecial, decimal taxaEsperada)
        {
            // Act
            decimal taxaCalculada = _contaCorrenteService.CalcularTaxaManutencao(saldo, ehEspecial);

            // Assert
            Assert.Equal(taxaEsperada, taxaCalculada);
        }

        [Theory]
        [InlineData(1000.00, 500.00, 1200.00, true)]
        [InlineData(1000.00, 500.00, 1600.00, false)]
        public void ValidarSaque_DeveRetornarResultadoEsperado(decimal saldo, decimal limite, decimal valorSaque, bool resultadoEsperado)
        {
            // Act
            bool resultado = _contaCorrenteService.ValidarSaque(saldo, limite, valorSaque);

            // Assert
            Assert.Equal(resultadoEsperado, resultado);
        }
    }
}